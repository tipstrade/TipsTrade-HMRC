using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TipsTrade.ApiClient.Core.Credential;
using TipsTrade.ApiClient.Core.Logging;
using TipsTrade.ApiClient.Core.Tenant;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.OAuth {
  /// <summary>Provides OAuth 2.0 authorisation flows for the HMRC API.</summary>
  public class HmrcOAuthService : IHmrcRestClient, IWithLogger {
    #region Fields
    private readonly HmrcOptions _options;
    private readonly Lazy<RestClient> _restclient;
    #endregion

    #region Lifecycle
    /// <summary>Initialises a new instance of <see cref="HmrcOAuthService"/>.</summary>
    public HmrcOAuthService(IHttpClientFactory httpClientFactory, IOptions<HmrcOptions> options,
      IHmrcAccessTokenProvider accessTokenProvider,
      IHmrcTenantProvider? tenantProvider = null, ILogger? logger = null
      ) {
      AccessTokenProvider = accessTokenProvider ?? throw new ArgumentNullException(nameof(accessTokenProvider));

      _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
      _restclient = this.BuildRestClient(httpClientFactory);

      TenantProvider = tenantProvider;
      Logger = logger;
    }
    #endregion

    #region Properties
    private IHmrcAccessTokenProvider AccessTokenProvider { get; }

    HmrcOptions IHmrcRestClient.Options => _options;

    Lazy<RestClient> IHmrcRestClient.RestClient => _restclient;

    private IHmrcTenantProvider? TenantProvider { get; }

    /// <inheritdoc/>
    public ILogger? Logger { get; }
    #endregion

    /// <summary>Gets the Uri for the Authorization endpoint.</summary>
    /// <param name="state">
    /// An opaque value used to maintain state between the request and callback and to prevent tampering as described in
    /// the OAuth 2.0 specification. This is passed back to your application via the redirect_uri.
    /// </param>
    /// <param name="redirectUrl">The URI that HMRC uses to send users back to your application after authorisation.</param>
    /// <param name="scopes">A list of scopes you would like to have permission to access on behalf of your user.</param>
    public string GetAuthorizationEndpoint(string state, Uri redirectUrl, IEnumerable<string> scopes) {
      if (scopes == null) {
        throw new ArgumentNullException(nameof(scopes));
      }

      return GetAuthorizationEndpoint(state, $"{redirectUrl}", scopes.ToArray());
    }

    /// <summary>Gets the Uri for the Authorization endpoint.</summary>
    /// <param name="state">
    /// An opaque value used to maintain state between the request and callback and to prevent tampering as described in
    /// the OAuth 2.0 specification. This is passed back to your application via the redirect_uri.
    /// </param>
    /// <param name="redirectUrl">The URI that HMRC uses to send users back to your application after authorisation.</param>
    /// <param name="scopes">A list of scopes you would like to have permission to access on behalf of your user.</param>
    public string GetAuthorizationEndpoint(string state, string redirectUrl, params string[] scopes) {
      if (string.IsNullOrEmpty(state)) {
        throw new ArgumentException($"{nameof(state)} cannot be empty.", nameof(state));
      } else if (string.IsNullOrEmpty(redirectUrl)) {
        throw new ArgumentException($"{nameof(redirectUrl)} cannot be empty.", nameof(redirectUrl));
      } else if (scopes == null) {
        throw new ArgumentNullException(nameof(scopes));
      } else if (scopes.Length == 0) {
        throw new ArgumentException($"{nameof(scopes)} cannot be empty.", nameof(scopes));
      }

      var options = this.GetOptions();
      var clientId = options.ClientId ?? throw new InvalidOperationException($"{nameof(options.ClientId)} must be provided in options.");

      var uri = new System.Text.StringBuilder(options.BaseUrl);
      uri.Append("/oauth/authorize?response_type=code");
      uri.Append($"&client_id={Uri.EscapeDataString(clientId)}");
      uri.Append($"&scope={Uri.EscapeDataString(string.Join(" ", scopes))}");
      uri.Append($"&state={Uri.EscapeDataString(state)}");
      uri.Append($"&redirect_uri={Uri.EscapeDataString(redirectUrl)}");

      return uri.ToString();
    }

    /// <summary>
    /// Exchanges the authorization code returned by the HMRC callback URI for a set of tokens.
    /// </summary>
    /// <param name="uri">The full callback URI that the Authorization endpoint redirected back to.</param>
    /// <param name="state">
    /// The opaque state value originally passed to <see cref="GetAuthorizationEndpoint(string, string, string[])"/>.
    /// The value is validated against the state returned in the callback to prevent CSRF.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The <see cref="TokenResponse"/> containing the access and refresh tokens.</returns>
    public async Task<TokenResponse> HandleEndpointResultAsync(string uri, string state, CancellationToken cancellationToken = default) {
      var u = new Uri(uri);
      var qs = HttpUtility.ParseQueryString(u.Query);

      // https://www.example.com/hmrc/callback?error=access_denied&error_description=user+denied+the+authorization&state=4f00d15e-de25-4796-999f-266ea4429889&error_code=USER_DENIED_AUTHORIZATION
      // https://www.example.com/hmrc/callback?code=51a0aa05968645a8ba609224e64ba28b&state=4f00d15e-de25-4796-999f-266ea4429889

      if (state != HttpUtility.UrlDecode(qs["state"])) {
        throw new InvalidOperationException($"{nameof(state)} does not match the returned value.");
      }

      if ("access_denied".Equals(qs["error"])) {
        var errorCode = HttpUtility.UrlDecode(qs["error_code"]);
        var errorMessage = HttpUtility.UrlDecode(qs["error_description"]);

        throw new ApiException(errorMessage ?? "OAuth2 error occurred.") {
          ApiError = new ErrorResponse() {
            Code = errorCode,
            Message = errorMessage
          }
        };
      }

      var options = this.GetOptions();
      var code = HttpUtility.UrlDecode(qs["code"]);
      var request = new RestRequest("oauth/token", Method.Post);
      request.AddParameter("client_secret", options.ClientSecret);
      request.AddParameter("client_id", options.ClientId);
      request.AddParameter("grant_type", "authorization_code");
      request.AddParameter("redirect_uri", $"{u.Scheme}://{u.Authority}{u.AbsolutePath}");
      request.AddParameter("code", code);

      var response = await this.GetRestClient().ExecuteAsync<TokenResponse>(request, cancellationToken).ConfigureAwait(false);
      response.ThrowOnError();

      return response.Data ?? throw new ApiException("Failed to obtain user tokens.");
    }

    /// <summary>
    /// Checks the current authentication state of the user by attempting to retrieve an access token from the <see cref="IHmrcAccessTokenProvider"/>.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple indicating whether the user has a token, whether it is valid, and the time remaining until it expires.</returns>
    public async Task<(bool HasToken, bool IsValid, TimeSpan ExpiresIn)> CheckUserAuthenticationStateAsync(CancellationToken cancellationToken = default) {
      string? tenantId = null;

      try {
        // This wraps the tenant retrieval in a try/catch to convert any exceptions into ApiExceptions with additional context for easier debugging.
        tenantId = await TenantProvider.GetTenantOrThrowAsync(cancellationToken).ConfigureAwait(false);
      } catch (InvalidOperationException ex) {
        throw new ApiException("No tenant could be identified for the current context.", ex);
      }

      try {
        // This wraps the token retrieval in a try/catch to convert any exceptions into ApiExceptions with additional context about the tenant for easier debugging.
        var token = await AccessTokenProvider.GetCredentialOrThrowAsync(tenantId, cancellationToken).ConfigureAwait (false);

        if (token == null) {
          return (false, false, TimeSpan.Zero);
        } else {
          var expiresIn = token.ExpiresTimestamp - DateTime.UtcNow;
          return (true, expiresIn > TimeSpan.Zero, expiresIn > TimeSpan.Zero ? expiresIn : TimeSpan.Zero);
        }
      } catch (InvalidOperationException ex) {
        throw new ApiException("Failed to obtain access token.", ex) {
          Data = { { "TenantId", tenantId } }
        };
      }
    }

    /// <summary>
    /// Exchanges a refresh token for a new access token. This is used to maintain access to the HMRC API on behalf of a user after the initial access token has expired.
    /// </summary>
    /// <param name="refreshToken">The refresh token obtained during the initial authorization.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="TokenResponse"/> containing the new access token and optionally a new refresh token.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided refresh token is null or empty.</exception>
    /// <exception cref="ApiException">Thrown when an error occurs while refreshing the access token.</exception>
    public async Task<TokenResponse> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken) {
      if (string.IsNullOrEmpty(refreshToken)) {
        throw new ArgumentException($"{nameof(refreshToken)} cannot be empty.", nameof(refreshToken));
      }

      var options = this.GetOptions();
      var restClient = this.GetRestClient();
      var request = new RestRequest("oauth/token", Method.Post);
      request.AddParameter("client_secret", options.ClientSecret);
      request.AddParameter("client_id", options.ClientId);
      request.AddParameter("grant_type", "refresh_token");
      request.AddParameter("refresh_token", refreshToken);

      var response = await restClient.ExecuteAsync<TokenResponse>(request, cancellationToken).ConfigureAwait(false);

      var oauthError = response.Content != null ? ErrorResponse.FromOAuth2Error(response.Content) : null;
      if (oauthError != null) {
        throw new ApiException(oauthError?.Message ?? "OAuth2 error occurred.") {
          ApiError = oauthError,
          Status = response?.StatusCode
        };
      }

      response.ThrowOnError();

      return response.Data ?? throw new ApiException("Failed to obtain user tokens.");
    }
  }
}
