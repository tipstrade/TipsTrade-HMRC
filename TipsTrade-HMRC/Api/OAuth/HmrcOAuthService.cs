using Microsoft.Extensions.Logging;
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
using TipsTrade.HMRC.Extensions;

namespace TipsTrade.HMRC.Api.OAuth {
  /// <summary>Provides OAuth 2.0 authorisation flows for the HMRC API.</summary>
  public class HmrcOAuthService : IWithLogger {
    #region Fields
    #endregion

    #region Lifecycle
    /// <summary>Initialises a new instance of <see cref="HmrcOAuthService"/>.</summary>
    public HmrcOAuthService(IHttpClientFactory httpClientFactory,
      IHmrcOptionsProvider hmrcOptionsProvider, IHmrcAccessTokenProvider accessTokenProvider,
      IHmrcTenantProvider? tenantProvider = null, ILogger? logger = null
      ) {
      HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
      AccessTokenProvider = accessTokenProvider ?? throw new ArgumentNullException(nameof(accessTokenProvider));

      Options = hmrcOptionsProvider ?? throw new ArgumentNullException(nameof(hmrcOptionsProvider));

      TenantProvider = tenantProvider;
      Logger = logger;

      RestClient = new Lazy<RestClient>(() => HttpClientFactory.CreateHmrcRestClient());
    }
    #endregion

    #region Properties
    private IHmrcAccessTokenProvider AccessTokenProvider { get; }

    private IHttpClientFactory HttpClientFactory { get; }

    private IHmrcOptionsProvider Options { get; }

    private Lazy<RestClient> RestClient { get; }

    private IHmrcTenantProvider? TenantProvider { get; }

    /// <inheritdoc/>
    public ILogger? Logger { get; }
    #endregion

    /// <summary>
    /// Generates the HMRC OAuth 2.0 authorization endpoint URL to which the user should be redirected to begin the authorization flow.
    /// </summary>
    /// <param name="state">An opaque value used to maintain state between the request and callback and to prevent tampering as described in the OAuth 2.0 specification. This is passed back to your application via the redirect_uri.</param>
    /// <param name="redirectUrl">The URI that HMRC uses to send users back to your application after authorisation.</param>
    /// <param name="scopes">A list of scopes you would like to have permission to access on behalf of your user.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The authorization endpoint URI.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string> GetAuthorizationEndpointAsync(string state, string redirectUrl, IEnumerable<string> scopes, CancellationToken cancellationToken = default) {
      if (string.IsNullOrEmpty(state)) {
        throw new ArgumentException($"{nameof(state)} cannot be empty.", nameof(state));
      } else if (string.IsNullOrEmpty(redirectUrl)) {
        throw new ArgumentException($"{nameof(redirectUrl)} cannot be empty.", nameof(redirectUrl));
      } else if (scopes == null) {
        throw new ArgumentNullException(nameof(scopes));
      }

      // Materialize scopes
      var scopesArray = scopes as string[] ?? scopes.ToArray();

      if (scopesArray.Length == 0) {
        throw new ArgumentException($"{nameof(scopes)} cannot be empty.", nameof(scopes));
      }

      var options = await Options.GetOptionsAsync(cancellationToken).ConfigureAwait(false);
      var clientId = options.ClientId ?? throw new InvalidOperationException($"{nameof(options.ClientId)} must be provided in options.");

      var uri = new System.Text.StringBuilder(options.BaseUrl);
      uri.Append("/oauth/authorize?response_type=code");
      uri.Append($"&client_id={Uri.EscapeDataString(clientId)}");
      uri.Append($"&scope={Uri.EscapeDataString(string.Join(" ", scopesArray))}");
      uri.Append($"&state={Uri.EscapeDataString(state)}");
      uri.Append($"&redirect_uri={Uri.EscapeDataString(redirectUrl)}");

      return uri.ToString();
    }

    /// <summary>
    /// Exchanges the authorization code returned by the HMRC callback URI for a set of tokens.
    /// </summary>
    /// <param name="uri">The full callback URI that the Authorization endpoint redirected back to.</param>
    /// <param name="state">
    /// The opaque state value originally passed to <see cref="GetAuthorizationEndpointAsync(string, string, IEnumerable{string}, CancellationToken)"/>.
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

      var options = await Options.GetOptionsAsync(cancellationToken).ConfigureAwait(false);
      var code = HttpUtility.UrlDecode(qs["code"]);

      var postUri = new Uri(new Uri(options.BaseUrl), "oauth/token");
      var request = new RestRequest(postUri, Method.Post);
      request.AddParameter("client_secret", options.ClientSecret);
      request.AddParameter("client_id", options.ClientId);
      request.AddParameter("grant_type", "authorization_code");
      request.AddParameter("redirect_uri", $"{u.Scheme}://{u.Authority}{u.AbsolutePath}");
      request.AddParameter("code", code);

      var response = await RestClient.Value.ExecuteAsync<TokenResponse>(request, cancellationToken).ConfigureAwait(false);
      response.ThrowOnError();

      return response.Data ?? throw new ApiException("Failed to obtain user tokens.").AddResponseData(response);
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
        throw new ApiException("No tenant could be identified for the current context.", ex).AddTenantId(tenantId);
      }

      try {
        // This wraps the token retrieval in a try/catch to convert any exceptions into ApiExceptions with additional context about the tenant for easier debugging.
        var token = await AccessTokenProvider.GetCredentialOrThrowAsync(tenantId, cancellationToken).ConfigureAwait(false);

        if (token == null) {
          return (false, false, TimeSpan.Zero);
        } else {
          var expiresIn = token.GetExpiresTimestamp() - DateTime.UtcNow;
          return (true, expiresIn > TimeSpan.Zero, expiresIn > TimeSpan.Zero ? expiresIn : TimeSpan.Zero);
        }
      } catch (InvalidOperationException ex) {
        throw new ApiException("Failed to obtain access token.", ex).AddTenantId(tenantId);
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
    public async Task<TokenResponse> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default) {
      if (string.IsNullOrEmpty(refreshToken)) {
        throw new ArgumentException($"{nameof(refreshToken)} cannot be empty.", nameof(refreshToken));
      }

      var options = await Options.GetOptionsAsync(cancellationToken).ConfigureAwait(false);
      var uri = new Uri(new Uri(options.BaseUrl), "oauth/token");
      var request = new RestRequest(uri, Method.Post);
      request.AddParameter("client_secret", options.ClientSecret);
      request.AddParameter("client_id", options.ClientId);
      request.AddParameter("grant_type", "refresh_token");
      request.AddParameter("refresh_token", refreshToken);

      var response = await RestClient.Value.ExecuteAsync<TokenResponse>(request, cancellationToken).ConfigureAwait(false);

      var oauthError = response.Content != null ? ErrorResponse.FromOAuth2Error(response.Content) : null;
      if (oauthError != null) {
        throw new ApiException(oauthError?.Message ?? "OAuth2 error occurred.").AddApiError(oauthError).AddResponseData(response);
      }

      response.ThrowOnError();

      return response.Data ?? throw new ApiException("Failed to obtain user tokens.").AddResponseData(response);
    }
  }
}
