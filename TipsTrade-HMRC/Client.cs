using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.CreateTestUser;
using TipsTrade.HMRC.Api.TestFraudPrevention;
using TipsTrade.HMRC.Api.HelloWorld;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Vat;
using TipsTrade.HMRC.Api.BusinessDetailsMtd;
using TipsTrade.HMRC.Api.ObligationsMtd;
using TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd;
using TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd;
using TipsTrade.HMRC.Api.IndividualCalculationsMtd;

namespace TipsTrade.HMRC {
  /// <summary>The Api client used to interact with the HMRC API.</summary>
  /// <remarks>This class is obsolete. Use the DI-based services registered via <c>IServiceCollection.AddHmrc()</c> instead.</remarks>
  [Obsolete("Client is obsolete. Use the DI-based services registered via IServiceCollection.AddHmrc() instead.")]
  public class Client {
    #region Fields
    /// <summary>The location of the production API.</summary>
    public const string ProductionUrl = "https://api.service.hmrc.gov.uk";

    /// <summary>The location of the sandbox API.</summary>
    public const string SandboxUrl = "https://test-api.service.hmrc.gov.uk";
    #endregion

    #region Properties
    /// <summary>The short-lived access token.</summary>
    public string AccessToken { get; set; }

    /// <summary>Gets or sets the information used to generate the anti fraud headers.</summary>
    public AntiFraud.AntiFraud AntiFraud { get; set; }

    private TokenResponse ApplicationToken { get; set; } = null;

    // Synchronization object used to make GetApplicationToken thread-safe
    private readonly object ApplicationTokenLock = new object();

    /// <summary>Gets the base Url used for all requests, based on the current environment.</summary>
    public string BaseUrl => IsSandbox ? SandboxUrl : ProductionUrl;

    /// <summary>The ID used to identify your application during each step of an OAuth 2.0 journey.</summary>
    public string ClientID { get; set; }

    /// <summary>The secret passphrase used to authorise your application during each step of an OAuth 2.0 journey. Keep it private.</summary>
    public string ClientSecret { get; set; }

    /// <summary>A flag indicating whether the client is accessing the sandbox environment.</summary>
    public bool IsSandbox { get; set; } = false;

    private JsonSerializerSettings JsonSettings { get; set; } = new JsonSerializerSettings() {
      ContractResolver = new DefaultContractResolver() {
        NamingStrategy = new CamelCaseNamingStrategy()
      }
    };

    /// <summary>The long-lived refresh token.</summary>
    public string RefreshToken { get; set; }

    /// <summary>The secret token used to authorise your application when making requests to any application-restricted endpoint.</summary>
    [Obsolete("The server token flow is no longer supported. Server tokens should not be used to authorise requests.")]
    public string ServerToken { get; set; }
    #endregion

    #region APIs
    /// <summary>The Business Details (MTD) API.</summary>
    public BusinessDetailsMtdService BusinessDetailsMtd => GetService<BusinessDetailsMtdService>();

    /// <summary>The Create Test User API.</summary>
    public CreateTestUserService CreateTestUser => GetService<CreateTestUserService>();

    /// <summary>The Hello World API.</summary>
    public HelloWorldService HelloWorld => GetService<HelloWorldService>();

    /// <summary>The Individual Calculations (MTD) API.</summary>
    public IndividualCalculationsMtdService IndividualCalculationsMtd => GetService<IndividualCalculationsMtdService>();

    /// <summary>The Obligations (MTD) API.</summary>
    public ObligationsMtdService ObligationsMtd => GetService<ObligationsMtdService>();

    /// <summary>The Self Assessment Test Support (MTD) API.</summary>
    public SelfAssessmentTestSupportMtdService SelfAssessmentTestSupportMtd => GetService<SelfAssessmentTestSupportMtdService>();

    /// <summary>The Self Employment Business (MTD) API.</summary>
    public SelfEmploymentBusinessMtdService SelfEmploymentBusinessMtd => GetService<SelfEmploymentBusinessMtdService>();

    /// <summary>The Test Fraud Prevention Headers API.</summary>
    public TestFraudPreventionService TestFraudPrevention => GetService<TestFraudPreventionService>();

    /// <summary>The VAT API.</summary>
    public VatService Vat => GetService<VatService>();

    /// <summary>The VAT Number API.</summary>
    public VatNumberService VatNumber => GetService<VatNumberService>();
    #endregion

    #region Constructors
    /// <summary>
    /// Creates an instance of the TipsTrade.HMRC.Client class.
    /// </summary>
    /// <param name="clientID">The ID used to identify your application during each step of an OAuth 2.0 journey.</param>
    /// <param name="clientSecret">The secret passphrase used to authorise your application during each step of an OAuth 2.0 journey.</param>
    /// <param name="serverToken">The server token used to authorise your application. This parameter is deprecated and will be removed in future versions.</param>
    /// <param name="isSandbox">A flag indicating whether the client is accessing the sandbox environment.</param>
    [Obsolete("The server token flow is no longer supported. Use the constructor that doesn't include the serverToken parameter.")]
    public Client(string clientID = null, string clientSecret = null, string serverToken = null, bool isSandbox = false) : this(clientID, clientSecret, isSandbox) {
      ServerToken = serverToken;
    }

    /// <summary>Creates an instance of the TipsTrade.HMRC.Client class.</summary>
    /// <param name="clientID">The ID used to identify your application during each step of an OAuth 2.0 journey.</param>
    /// <param name="clientSecret">The secret passphrase used to authorise your application during each step of an OAuth 2.0 journey.</param>
    /// <param name="isSandbox">A flag indicating whether the client is accessing the sandbox environment.</param>
    public Client(string clientID = null, string clientSecret = null, bool isSandbox = false) {
      ClientID = clientID;
      ClientSecret = clientSecret;
      IsSandbox = isSandbox;
    }
    #endregion

    #region Methods
    /// <summary>Builds an <see cref="HmrcOptions"/> snapshot from the current client state.</summary>
    private HmrcOptions BuildOptions() => new HmrcOptions {
      AccessToken = AccessToken,
      AntiFraud = AntiFraud,
      ClientID = ClientID,
      ClientSecret = ClientSecret,
      IsSandbox = IsSandbox,
      RefreshToken = RefreshToken
    };

    /// <summary>Creates a new instance of the specified service type using the current client options.</summary>
    private T GetService<T>() where T : HmrcServiceBase {
      return (T)Activator.CreateInstance(typeof(T), BuildOptions());
    }

    /// <summary>Gets the Uri for the Authorization endpoint.</summary>
    /// <param name="state">
    /// An opaque value used to maintain state between the request and callback and to prevent tampering as described in
    /// the OAuth 2.0 specification (opens in a new tab). This is passed back to your application via the redirect_uri.
    /// </param>
    /// <param name="redirectUrl">	The URI that we use to send users back to your application after successful (or unsuccessful) authorisation.</param>
    /// <param name="scopes">	A list of scopes you would like to have permission to access on behalf of your user.</param>
    /// <returns></returns>
    [Obsolete("Use GetAuthorizationEndpoint instead.")]
    public string GetAuthorizatoinEndpoint(string state, Uri redirectUrl, IEnumerable<string> scopes) {
      return GetAuthorizationEndpoint(state, $"{redirectUrl}", scopes?.ToArray());
    }

    /// <summary>Gets the Uri for the Authorization endpoint.</summary>
    /// <param name="state">
    /// An opaque value used to maintain state between the request and callback and to prevent tampering as described in
    /// the OAuth 2.0 specification (opens in a new tab). This is passed back to your application via the redirect_uri.
    /// </param>
    /// <param name="redirectUrl">	The URI that we use to send users back to your application after successful (or unsuccessful) authorisation.</param>
    /// <param name="scopes">	A list of scopes you would like to have permission to access on behalf of your user.</param>
    /// <returns></returns>
    public string GetAuthorizationEndpoint(string state, Uri redirectUrl, IEnumerable<string> scopes) {
      return GetAuthorizationEndpoint(state, $"{redirectUrl}", scopes?.ToArray());
    }

    /// <summary>Gets the Uri for the Authorization endpoint.</summary>
    /// <param name="state">
    /// An opaque value used to maintain state between the request and callback and to prevent tampering as described in
    /// the OAuth 2.0 specification (opens in a new tab). This is passed back to your application via the redirect_uri.
    /// </param>
    /// <param name="redirectUrl">	The URI that we use to send users back to your application after successful (or unsuccessful) authorisation.</param>
    /// <param name="scopes">	A list of scopes you would like to have permission to access on behalf of your user.</param>
    /// <returns></returns>
    [Obsolete("Use GetAuthorizationEndpoint instead.")]
    public string GetAuthorizatoinEndpoint(string state, string redirectUrl, params string[] scopes) {
      return GetAuthorizationEndpoint(state, redirectUrl, scopes);
    }


    /// <summary>Gets the Uri for the Authorization endpoint.</summary>
    /// <param name="state">
    /// An opaque value used to maintain state between the request and callback and to prevent tampering as described in
    /// the OAuth 2.0 specification (opens in a new tab). This is passed back to your application via the redirect_uri.
    /// </param>
    /// <param name="redirectUrl">	The URI that we use to send users back to your application after successful (or unsuccessful) authorisation.</param>
    /// <param name="scopes">	A list of scopes you would like to have permission to access on behalf of your user.</param>
    /// <returns></returns>
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

      var uri = new System.Text.StringBuilder(BaseUrl);
      uri.Append("/oauth/authorize?response_type=code");
      uri.Append($"&client_id={HttpUtility.UrlEncode(ClientID)}");
      uri.Append($"&scope={HttpUtility.UrlEncode(string.Join(" ", scopes))}");
      uri.Append($"&state={HttpUtility.UrlEncode(state)}");
      uri.Append($"&redirect_uri={HttpUtility.UrlEncode(redirectUrl)}");

      return uri.ToString();
    }

    /// <summary>
    /// Gets an application access token using the client credentials flow. This is used to access APIs that don't require user context. If the token has expired, a new one will be requested and cached for future use.
    /// </summary>
    /// <returns>The application access token.</returns>
    /// <exception cref="ApiException">Thrown when the token request fails.</exception>
    internal TokenResponse GetApplicationToken() {
      // This is a bit messy. The client should probably be passing the Application Access Token around to the APIs that need it, but this is a bit easier to implement for now.
      // The token is cached in memory and will be reused until it expires, at which point a new token will be requested.

      // Fast path: if another thread already refreshed the token, return it without locking
      if (ApplicationToken != null && !ApplicationToken.HasAccessTokenExpired()) {
        return ApplicationToken;
      }

      // Ensure only one thread requests a new token at a time
      lock (ApplicationTokenLock) {
        // Re-check inside the lock in case another thread refreshed while we were waiting
        if (ApplicationToken != null && !ApplicationToken.HasAccessTokenExpired()) {
          return ApplicationToken;
        }

        var restClient = new RestClient(BaseUrl);
        var request = new RestRequest("oauth/token", Method.Post);
        request.AddParameter("client_secret", ClientSecret);
        request.AddParameter("client_id", ClientID);
        request.AddParameter("grant_type", "client_credentials");

        var response = restClient.Execute<TokenResponse>(request);

        // The OAuth2 flow returns different JSON in the event of an error. Check for that first
        var oauthError = ErrorResponse.FromOAuth2Error(response.Content);
        if (oauthError != null) {
          throw new ApiException(oauthError.Message) {
            ApiError = oauthError,
            Status = response.StatusCode
          };
        }

        response.ThrowOnError();

        ApplicationToken = response.Data ?? throw new ApiException("Failed to obtain application token.");

        return ApplicationToken;
      }
    }

    /// <summary></summary>
    /// <param name="uri">The Uri that the Authorization endpoint redirected back to.</param>
    /// <param name="state">
    /// An opaque value used to maintain state between the request and callback and to prevent tampering as described in
    /// the OAuth 2.0 specification (opens in a new tab). This is passed back to your application via the redirect_uri.
    /// </param>
    /// <returns></returns>
    public TokenResponse HandleEndpointResult(string uri, string state) {
      var u = new Uri(uri);
      var qs = HttpUtility.ParseQueryString(u.Query);

      // https://www.example.com/hmrc/callback?error=access_denied&error_description=user+denied+the+authorization&state=4f00d15e-de25-4796-999f-266ea4429889&error_code=USER_DENIED_AUTHORIZATION
      // https://www.example.com/hmrc/callback?code=51a0aa05968645a8ba609224e64ba28b&state=4f00d15e-de25-4796-999f-266ea4429889

      // State must be valid
      if (state != HttpUtility.UrlDecode(qs["state"])) {
        throw new InvalidOperationException($"{nameof(state)} does not match the returned value.");
      }

      // Was an error returned
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

      var restClient = new RestClient(BaseUrl);

      var request = new RestRequest("oauth/token", Method.Post);
      request.AddParameter("client_secret", ClientSecret);
      request.AddParameter("client_id", ClientID);
      request.AddParameter("grant_type", "authorization_code");
      request.AddParameter("redirect_uri", $"{u.Scheme}://{u.Authority}{u.AbsolutePath}");
      request.AddParameter("code", code);

      var response = restClient.Execute<TokenResponse>(request);
      response.ThrowOnError();

      var tokens = response.Data ?? throw new ApiException("Failed to obtain user tokens.");
      AccessToken = tokens.AccessToken;
      RefreshToken = tokens.RefreshToken;

      return tokens;
    }

    /// <summary>Refresh the user's access token using the specified refresh token.</summary>
    /// <param name="refreshToken">The user's refresh token. This in a one-use token and will expire immediately.</param>
    public TokenResponse RefreshAccessToken(string refreshToken) {
      if (string.IsNullOrEmpty(refreshToken)) {
        throw new ArgumentException($"{nameof(refreshToken)} cannot be empty.", nameof(refreshToken));
      }

      var restClient = new RestClient(BaseUrl);

      var request = new RestRequest("oauth/token", Method.Post);
      request.AddParameter("client_secret", ClientSecret);
      request.AddParameter("client_id", ClientID);
      request.AddParameter("grant_type", "refresh_token");
      request.AddParameter("refresh_token", refreshToken);

      var response = restClient.Execute<TokenResponse>(request);

      // The OAuth2 flow returns different JSON in the event of an error. Check for that first
      var oauthError = ErrorResponse.FromOAuth2Error(response.Content);
      if (oauthError != null) {
        throw new ApiException(oauthError.Message) {
          ApiError = oauthError,
          Status = response.StatusCode
        };
      }

      response.ThrowOnError();

      var tokens = response.Data ?? throw new ApiException("Failed to obtain user tokens.");
      AccessToken = tokens.AccessToken;
      RefreshToken = tokens.RefreshToken;

      return tokens;
    }
    #endregion
  }
}
