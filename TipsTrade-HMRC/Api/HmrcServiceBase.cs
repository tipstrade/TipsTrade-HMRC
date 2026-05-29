using Microsoft.Extensions.Options;
using RestSharp;
using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api {
  /// <summary>Base class for all HMRC API services, providing shared configuration and token management.</summary>
  public abstract class HmrcServiceBase : IHmrcService {
    private TokenResponse _applicationToken;
    private readonly object _applicationTokenLock = new object();

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

    /// <summary>Initialises a new instance of <see cref="HmrcServiceBase"/> using an <see cref="IOptions{HmrcOptions}"/> instance.</summary>
    protected HmrcServiceBase(IOptions<HmrcOptions> options) {
      Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>Initialises a new instance of <see cref="HmrcServiceBase"/> using a plain <see cref="HmrcOptions"/> instance.</summary>
    protected HmrcServiceBase(HmrcOptions options) {
      Options = options ?? throw new ArgumentNullException(nameof(options));
    }

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

        var restClient = new RestClient(Options.BaseUrl);
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
  }
}
