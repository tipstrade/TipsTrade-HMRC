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
using TipsTrade.HMRC.FraudPrevention;

namespace TipsTrade.HMRC.Api {
  /// <summary>Base class for all HMRC API services, providing shared configuration and token management.</summary>
  public abstract class HmrcServiceBase : IHmrcRestClient, IWithLogger {
    #region Fields
    private static readonly AsyncKeyedLocker<string> _tokenLocks = new AsyncKeyedLocker<string>();
    private HmrcOptions _options;
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
      _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
      _restClient = this.BuildRestClient(httpClientFactory);

      AccessTokenProvider = accessTokenProvider ?? throw new ArgumentNullException(nameof(accessTokenProvider));
      ApplicationTokenCache = applicationTokenCache ?? throw new ArgumentNullException(nameof(applicationTokenCache));
      OauthService = oauthService ?? throw new ArgumentNullException(nameof(oauthService));

      // Tenant provider is optional and may not be needed for all services, so if it's not provided we use a default implementation that returns null
      TenantProvider = tenantProvider;
      Logger = logger;
    }
    #endregion

    #region Properties
    /// <summary>
    /// An <see cref="IHmrcAccessTokenProvider"/> implementation responsible for providing user access tokens for API requests that require user-level authorization.
    /// </summary>
    private IHmrcAccessTokenProvider AccessTokenProvider { get; }

    /// <summary>
    /// An in-memory cache for application tokens obtained via the client credentials flow.
    /// This cache is used to store and retrieve application tokens to avoid unnecessary token requests and to handle token expiration.
    /// </summary>
    private ApplicationTokenCache ApplicationTokenCache { get; }

    /// <inheritdoc/>
    public ILogger? Logger { get; }

    /// <summary>
    /// An <see cref="HmrcOAuthService"/> instance used to perform OAuth 2.0 token refresh operations when user access tokens expire, as well as to generate authorization URLs for user consent flows.
    /// </summary>
    private HmrcOAuthService OauthService { get; }

    /// <summary>
    /// An optional provider for resolving the tenant context in multi-tenant applications.
    /// If not supplied, a default implementation is used that returns a single default tenant ID, effectively treating the application as single-tenant.
    /// </summary>
    private IHmrcTenantProvider? TenantProvider { get; }
    #endregion

    #region IHmrcRestClient implementation
    /// <inheritdoc/>
    HmrcOptions IHmrcRestClient.Options => _options;

    /// <inheritdoc/>
    Lazy<RestClient> IHmrcRestClient.RestClient => _restClient;
    #endregion

    #region IHmrcService implementation
    /// <summary>The description of the API.</summary>
    public abstract string Description { get; }

    /// <summary>A flag indicating whether this version of the API is stable.</summary>
    public abstract bool IsStable { get; }

    /// <summary>The relative location of the API.</summary>
    public abstract string Location { get; }

    /// <summary>The name of the API.</summary>
    public abstract string Name { get; }

    /// <summary>The version of the API that the client should target.</summary>
    public abstract string Version { get; }
    #endregion

    #region Private methods
    [Obsolete("Use CreateRequestAsync instead. Synchronous methods may cause deadlocks.")]
    internal RestRequest CreateRequest(IApiRequest request) {
      return CreateRequestAsync(request, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Create and populate a <see cref="RestRequest"/> from a given <see cref="IApiRequest"/> using API client settings.
    /// </summary>
    /// <param name="request">The request model that will populate headers, body and parameters.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the async operation.</param>
    /// <returns>A fully populated <see cref="RestRequest"/> ready for execution.</returns>
    /// <exception cref="ApiException">
    /// Thrown if required configuration is missing for the request, such as credentials for authorization or fraud prevention headers.
    /// </exception>
    internal async Task<RestRequest> CreateRequestAsync(IApiRequest request, CancellationToken cancellationToken) {
      var options = this.GetOptions();
      var restRequest = new RestRequest($"{Location}/{request.Location}", request.Method);
      restRequest.AddHeader("Accept", GetAcceptHeader(request.AcceptType));

      if (options.IsSandbox && request is IGovTestScenario govTest) {
        restRequest.AddGovTestScenario(govTest);
      }

      if (request is IDateRange dateRange) {
        restRequest.AddDateRangeParameters(dateRange);
      }

      if (request.Authorization == Authorization.Application) {
        var token = await GetApplicationTokenAsync(cancellationToken);
        restRequest.AddHeader("Authorization", $"Bearer {token}");

      } else if (request.Authorization == Authorization.User) {
        var accessToken = await GetAccessTokenAsync(cancellationToken);

        restRequest.AddHeader("Authorization", $"Bearer {accessToken}");
      }

      if (this is IRequiresFraudPrevention) {
        if (options.FraudPreventionConfig == null) {
          throw new ApiException("The request requires fraud prevention headers, but the client's FraudPrevention configuration is null.");
        }

        options.FraudPreventionConfig.AddHeadersToRequest(restRequest);
      }

      if (request is IApiRequestWithParameters requestWithParameters) {
        requestWithParameters.PopulateRequestParameters(restRequest);
      }

      if (request is IApiRequestWithBody requestWithBody) {
        restRequest.AddHeader("Content-Type", requestWithBody.ContentType);
        requestWithBody.PopulateRequestBody(restRequest);
      }

      return restRequest;
    }

    /// <summary>
    /// Create a <see cref="RestRequest"/> from the supplied <see cref="IApiRequest"/> and execute it synchronously,
    /// deserializing the response into <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The expected response model type.</typeparam>
    /// <param name="request">The request model used to create the HTTP request.</param>
    /// <returns>An instance of <typeparamref name="T"/> representing the API response.</returns>
    [Obsolete("Use ExecuteRequestAsync instead. Synchronous methods may cause deadlocks.")]
    internal T ExecuteRequest<T>(IApiRequest request) where T : class, new() {
      var restRequest = CreateRequest(request);

      return ExecuteRequest<T>(restRequest);
    }

    /// <summary>
    /// Execute the specified <see cref="RestRequest"/> synchronously and handle the response.
    /// </summary>
    /// <typeparam name="T">The expected response model type.</typeparam>
    /// <param name="request">The <see cref="RestRequest"/> to execute.</param>
    /// <returns>An instance of <typeparamref name="T"/> representing the API response.</returns>
    [Obsolete("Use ExecuteRequestAsync instead. Synchronous methods may cause deadlocks.")]
    internal T ExecuteRequest<T>(RestRequest request) where T : class, new() {
      var response = this.GetRestClient().Execute<T>(request);

      return response.HandleResponse();
    }

    /// <summary>
    /// Create a <see cref="RestRequest"/> from the supplied <see cref="IApiRequest"/> and execute it asynchronously,
    /// deserializing the response into <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The expected response model type.</typeparam>
    /// <param name="request">The request model used to create the HTTP request.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the async operation.</param>
    /// <returns>A task that resolves to an instance of <typeparamref name="T"/> representing the API response.</returns>
    internal async Task<T> ExecuteRequestAsync<T>(IApiRequest request, CancellationToken cancellationToken) where T : class, new() {
      var restRequest = await CreateRequestAsync(request, cancellationToken).ConfigureAwait(false);

      return await ExecuteRequestAsync<T>(restRequest, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Execute the specified <see cref="RestRequest"/> asynchronously and handle the response.
    /// </summary>
    /// <typeparam name="T">The expected response model type.</typeparam>
    /// <param name="request">The <see cref="RestRequest"/> to execute.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the async operation.</param>
    /// <returns>A task that resolves to an instance of <typeparamref name="T"/> representing the API response.</returns>
    internal async Task<T> ExecuteRequestAsync<T>(RestRequest request, CancellationToken cancellationToken) where T : class, new() {
      var response = await this.GetRestClient().ExecuteAsync<T>(request, cancellationToken).ConfigureAwait(false);

      return response.HandleResponse();
    }

    /// <summary>
    /// Gets the versioned Accept header required by the HMRC API.
    /// </summary>
    /// <param name="contentType">The optional content type to be accepted (usually json).</param>
    /// <returns>A string containing a valid HTTP Accept header for the HMRC API versioning scheme.</returns>
    /// <remarks>
    /// See HMRC API versioning guidance: <see href="https://developer.service.hmrc.gov.uk/api-documentation/docs/reference-guide#versioning" />
    /// </remarks>
    internal string GetAcceptHeader(string contentType) {
      return $"application/vnd.hmrc.{Version}+{contentType}";
    }

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
      var options = this.GetOptions();
      var clientId = options.ClientID ?? throw new InvalidOperationException("ClientID must be configured to obtain an application token.");
      var clientSecret = options.ClientSecret ?? throw new InvalidOperationException("ClientSecret must be configured to obtain an application token.");

      // Short circuit if we have a valid cached token to avoid unnecessary locking and HTTP calls
      var cached = ApplicationTokenCache.Get(clientId);
      if (cached != null) {
        return cached.AccessToken;
      }

      using (await _tokenLocks.LockAsync(clientId, cancellationToken)) {
        // Check the cache again inside the lock in case another thread already refreshed the token while we were waiting
        cached = ApplicationTokenCache.Get(clientId);
        if (cached != null) {
          return cached.AccessToken;
        }

        var request = new RestRequest("oauth/token", Method.Post);
        request.AddParameter("client_secret", clientSecret);
        request.AddParameter("client_id", clientId);
        request.AddParameter("grant_type", "client_credentials");

        var response = await this.GetRestClient().ExecuteAsync<TokenResponse>(request, cancellationToken).ConfigureAwait(false);

        var oauthError = response.Content != null ? ErrorResponse.FromOAuth2Error(response.Content) : null;
        if (oauthError != null) {
          throw new ApiException(oauthError?.Message ?? "OAuth2 error occurred.") {
            ApiError = oauthError,
            Status = response?.StatusCode
          };
        }

        response.ThrowOnError();

        var token = response.Data ?? throw new ApiException("Failed to obtain application token.");
        ApplicationTokenCache.Set(clientId, token);

        return token.AccessToken;
      }
    }
    #endregion
  }
}
