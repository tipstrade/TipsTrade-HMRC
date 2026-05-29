using AsyncKeyedLock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.ApiClient.Core.Logging;
using TipsTrade.ApiClient.Core.Tenant;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.OAuth;

namespace TipsTrade.HMRC.Api {
  /// <summary>Base class for all HMRC API services, providing shared configuration and token management.</summary>
  public abstract class HmrcServiceBase : IHmrcService, IWithLogger {
    #region Fields
    /// <summary>The name used to register the named <see cref="HttpClient"/> for HMRC API calls.</summary>
    internal static readonly string HttpClientName = typeof(HmrcServiceBase).FullName ?? typeof(HmrcServiceBase).Name;

    private static readonly AsyncKeyedLocker<string> _tokenLocks = new AsyncKeyedLocker<string>();

    private Lazy<RestClient> _restClient;
    #endregion

    #region Lifecycle
    /// <summary>Initialises a new instance of <see cref="HmrcServiceBase"/> using an <see cref="IOptions{HmrcOptions}"/> instance.</summary>
    protected HmrcServiceBase(
      IOptions<HmrcOptions> options,
      IHttpClientFactory httpClientFactory,
      IHmrcAccessTokenProvider accessTokenProvider, ApplicationTokenCache applicationTokenCache, HmrcOAuthService oauthService,
      IHmrcTenantProvider? tenantProvider = null,
      ILogger? logger = null
      ) {
      Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
      AccessTokenProvider = accessTokenProvider ?? throw new ArgumentNullException(nameof(accessTokenProvider));
      ApplicationTokenCache = applicationTokenCache ?? throw new ArgumentNullException(nameof(applicationTokenCache));
      OauthService = oauthService ?? throw new ArgumentNullException(nameof(oauthService));

      // Tenant provider is optional and may not be needed for all services, so if it's not provided we use a default implementation that returns null
      TenantProvider = tenantProvider;
      Logger = logger;

      _restClient = new Lazy<RestClient>(() => {
        var httpClient = httpClientFactory.CreateClient(HttpClientName);
        return new RestClient(httpClient, new RestClientOptions(Options.BaseUrl));
      });
    }
    #endregion

    #region Properties
    private IHmrcAccessTokenProvider AccessTokenProvider { get; }
    private ApplicationTokenCache ApplicationTokenCache { get; }
    private HmrcOAuthService OauthService { get; }
    /// <inheritdoc/>
    public ILogger? Logger { get; }
    private IHmrcTenantProvider? TenantProvider { get; }
    /// <summary>Gets the shared <see cref="RestClient"/> backed by the named <see cref="HttpClient"/> from the factory.</summary>
    internal RestClient RestClient => _restClient.Value;
    #endregion

    #region IHmrcService implementation
    /// <inheritdoc/>
    public HmrcOptions Options { get; }

    /// <inheritdoc/>
    public abstract string Description { get; }

    /// <inheritdoc/>
    public abstract bool IsStable { get; }

    /// <inheritdoc/>
    public abstract string Location { get; }

    /// <inheritdoc/>
    public abstract string Name { get; }

    /// <inheritdoc/>
    public abstract string Version { get; }
    #endregion

    #region Private methods
    /// <summary>
    /// Gets the tenant ID for the current context using the tenant provider, or "(default)" if no tenant provider is configured.
    /// </summary>
    protected async Task<string> GetTenantAsync(CancellationToken cancellationToken) {
      return await TenantProvider.GetTenantOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Gets an access token for the current tenant, refreshing it if necessary. The token is obtained from the <see cref="IHmrcAccessTokenProvider"/> which may implement its own caching strategy.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="TokenResponse"/> containing the access token and refresh token.</returns>
    /// <exception cref="ApiException">Thrown if no access token is found or if the token refresh fails.</exception>
    internal async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken) {
      var tenantId = await GetTenantAsync(cancellationToken);

      var token = await AccessTokenProvider.GetCredentialAsync(tenantId, cancellationToken).ConfigureAwait(false);

      if (token == null) {
        Logger?.LogInformation("No access token found for tenant '{TenantId}'.", tenantId);
        throw new ApiException("No access token found. Ensure that the tenant has been configured with valid credentials.");
      }

      if (token.HasAccessTokenExpired()) {
        token = await OauthService.RefreshAccessTokenAsync(token.RefreshToken, cancellationToken).ConfigureAwait(false);
      }

      return token.AccessToken;
    }

    /// <summary>
    /// Gets (or refreshes) an application-level access token using the client credentials flow.
    /// The token is cached in memory and reused until it expires.
    /// </summary>
    internal async Task<string> GetApplicationTokenAsync(CancellationToken cancellationToken) {
      // Short circuit if we have a valid cached token to avoid unnecessary locking and HTTP calls
      var cached = ApplicationTokenCache.Get(Options.ClientID);
      if (cached != null) {
        return cached.AccessToken;
      }

      using (await _tokenLocks.LockAsync(Options.ClientID, cancellationToken)) {
        // Check the cache again inside the lock in case another thread already refreshed the token while we were waiting
        cached = ApplicationTokenCache.Get(Options.ClientID);
        if (cached != null) {
          return cached.AccessToken;
        }

        var request = new RestRequest("oauth/token", Method.Post);
        request.AddParameter("client_secret", Options.ClientSecret);
        request.AddParameter("client_id", Options.ClientID);
        request.AddParameter("grant_type", "client_credentials");

        var response = await RestClient.ExecuteAsync<TokenResponse>(request, cancellationToken).ConfigureAwait(false);

        var oauthError = ErrorResponse.FromOAuth2Error(response.Content);
        if (oauthError != null) {
          throw new ApiException(oauthError.Message) {
            ApiError = oauthError,
            Status = response.StatusCode
          };
        }

        response.ThrowOnError();

        var token = response.Data ?? throw new ApiException("Failed to obtain application token.");
        ApplicationTokenCache.Set(Options.ClientID, token);

        return token.AccessToken;
      }
    }
    #endregion
  }
}
