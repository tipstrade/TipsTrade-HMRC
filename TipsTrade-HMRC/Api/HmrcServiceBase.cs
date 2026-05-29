using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Net.Http;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api {
  /// <summary>Base class for all HMRC API services, providing shared configuration and token management.</summary>
  public abstract class HmrcServiceBase : IHmrcService {
    #region Fields
    /// <summary>The name used to register the named <see cref="HttpClient"/> for HMRC API calls.</summary>
    internal static readonly string HttpClientName = typeof(HmrcServiceBase).FullName;

    private TokenResponse _applicationToken;
    private readonly object _applicationTokenLock = new object();
    private readonly IHttpClientFactory _httpClientFactory;
    private Lazy<RestClient> _restClient;
    #endregion

    #region Lifecycle
    /// <summary>Initialises a new instance of <see cref="HmrcServiceBase"/> using an <see cref="IOptions{HmrcOptions}"/> instance.</summary>
    protected HmrcServiceBase(IOptions<HmrcOptions> options, IHttpClientFactory httpClientFactory) {
      Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
      _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
      _restClient = new Lazy<RestClient>(() => {
        var httpClient = _httpClientFactory.CreateClient(HttpClientName);
        return new RestClient(httpClient, new RestClientOptions(Options.BaseUrl));
      });
    }

    /// <summary>Initialises a new instance of <see cref="HmrcServiceBase"/> using a plain <see cref="HmrcOptions"/> instance.</summary>
    protected HmrcServiceBase(HmrcOptions options, IHttpClientFactory httpClientFactory) {
      Options = options ?? throw new ArgumentNullException(nameof(options));
      _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

      _restClient = new Lazy<RestClient>(() => {
        var httpClient = _httpClientFactory.CreateClient(HttpClientName);
        return new RestClient(httpClient, new RestClientOptions(Options.BaseUrl));
      });
    }
    #endregion

    #region Properties
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
    /// <summary>Gets the shared <see cref="RestClient"/> backed by the named <see cref="HttpClient"/> from the factory.</summary>
    internal RestClient RestClient => _restClient.Value;

    /// <summary>
    /// Gets (or refreshes) an application-level access token using the client credentials flow.
    /// The token is cached in memory and reused until it expires.
    /// </summary>
    internal TokenResponse GetApplicationToken() {
      if (_applicationToken != null && !_applicationToken.HasAccessTokenExpired()) {
        return _applicationToken;
      }

      lock (_applicationTokenLock) {
        if (_applicationToken != null && !_applicationToken.HasAccessTokenExpired()) {
          return _applicationToken;
        }

        var restClient = RestClient;
        var request = new RestRequest("oauth/token", Method.Post);
        request.AddParameter("client_secret", Options.ClientSecret);
        request.AddParameter("client_id", Options.ClientID);
        request.AddParameter("grant_type", "client_credentials");

        var response = restClient.Execute<TokenResponse>(request);

        var oauthError = ErrorResponse.FromOAuth2Error(response.Content);
        if (oauthError != null) {
          throw new ApiException(oauthError.Message) {
            ApiError = oauthError,
            Status = response.StatusCode
          };
        }

        response.ThrowOnError();

        _applicationToken = response.Data ?? throw new ApiException("Failed to obtain application token.");

        return _applicationToken;
      }
    }
    #endregion
  }
}
