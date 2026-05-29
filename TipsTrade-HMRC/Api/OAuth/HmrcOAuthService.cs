using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.OAuth {
  /// <summary>Provides OAuth 2.0 authorisation flows for the HMRC API.</summary>
  public class HmrcOAuthService {
    private readonly HmrcOptions _options;

    /// <summary>Initialises a new instance of <see cref="HmrcOAuthService"/>.</summary>
    public HmrcOAuthService(IOptions<HmrcOptions> options) {
      _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>Gets the Uri for the Authorization endpoint.</summary>
    /// <param name="state">
    /// An opaque value used to maintain state between the request and callback and to prevent tampering as described in
    /// the OAuth 2.0 specification. This is passed back to your application via the redirect_uri.
    /// </param>
    /// <param name="redirectUrl">The URI that HMRC uses to send users back to your application after authorisation.</param>
    /// <param name="scopes">A list of scopes you would like to have permission to access on behalf of your user.</param>
    public string GetAuthorizationEndpoint(string state, Uri redirectUrl, IEnumerable<string> scopes) {
      return GetAuthorizationEndpoint(state, $"{redirectUrl}", scopes?.ToArray());
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
      }
      if (string.IsNullOrEmpty(redirectUrl)) {
        throw new ArgumentException($"{nameof(redirectUrl)} cannot be empty.", nameof(redirectUrl));
      }
      if (scopes == null) {
        throw new ArgumentNullException(nameof(scopes));
      }
      if (scopes.Length == 0) {
        throw new ArgumentException($"{nameof(scopes)} cannot be empty.", nameof(scopes));
      }

      var uri = new System.Text.StringBuilder(_options.BaseUrl);
      uri.Append("/oauth/authorize?response_type=code");
      uri.Append($"&client_id={Uri.EscapeDataString(_options.ClientID)}");
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
    /// <returns>The <see cref="TokenResponse"/> containing the access and refresh tokens.</returns>
    public TokenResponse HandleEndpointResult(string uri, string state) {
      var u = new Uri(uri);
      var qs = HttpUtility.ParseQueryString(u.Query);

      // https://www.example.com/hmrc/callback?error=access_denied&error_description=user+denied+the+authorization&state=4f00d15e-de25-4796-999f-266ea4429889&error_code=USER_DENIED_AUTHORIZATION
      // https://www.example.com/hmrc/callback?code=51a0aa05968645a8ba609224e64ba28b&state=4f00d15e-de25-4796-999f-266ea4429889

      if (state != HttpUtility.UrlDecode(qs["state"])) {
        throw new InvalidOperationException($"{nameof(state)} does not match the returned value.");
      }

      if ("access_denied".Equals(qs["error"])) {
        string errorCode = HttpUtility.UrlDecode(qs["error_code"]);
        var errorMessage = HttpUtility.UrlDecode(qs["error_description"]);
        throw new ApiException(errorMessage) {
          ApiError = new ErrorResponse() {
            Code = errorCode,
            Message = errorMessage
          }
        };
      }

      var code = HttpUtility.UrlDecode(qs["code"]);

      var restClient = new RestClient(_options.BaseUrl);
      var request = new RestRequest("oauth/token", Method.Post);
      request.AddParameter("client_secret", _options.ClientSecret);
      request.AddParameter("client_id", _options.ClientID);
      request.AddParameter("grant_type", "authorization_code");
      request.AddParameter("redirect_uri", $"{u.Scheme}://{u.Authority}{u.AbsolutePath}");
      request.AddParameter("code", code);

      var response = restClient.Execute<TokenResponse>(request);
      response.ThrowOnError();

      return response.Data ?? throw new ApiException("Failed to obtain user tokens.");
    }

    /// <summary>Refreshes the user's access token using the specified refresh token.</summary>
    /// <param name="refreshToken">The user's refresh token. This is a one-use token and will expire immediately.</param>
    /// <returns>The <see cref="TokenResponse"/> containing the new access and refresh tokens.</returns>
    public TokenResponse RefreshAccessToken(string refreshToken) {
      if (string.IsNullOrEmpty(refreshToken)) {
        throw new ArgumentException($"{nameof(refreshToken)} cannot be empty.", nameof(refreshToken));
      }

      var restClient = new RestClient(_options.BaseUrl);
      var request = new RestRequest("oauth/token", Method.Post);
      request.AddParameter("client_secret", _options.ClientSecret);
      request.AddParameter("client_id", _options.ClientID);
      request.AddParameter("grant_type", "refresh_token");
      request.AddParameter("refresh_token", refreshToken);

      var response = restClient.Execute<TokenResponse>(request);

      var oauthError = ErrorResponse.FromOAuth2Error(response.Content);
      if (oauthError != null) {
        throw new ApiException(oauthError.Message) {
          ApiError = oauthError,
          Status = response.StatusCode
        };
      }

      response.ThrowOnError();

      return response.Data ?? throw new ApiException("Failed to obtain user tokens.");
    }
  }
}
