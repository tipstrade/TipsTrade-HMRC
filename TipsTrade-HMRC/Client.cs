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

namespace TipsTrade.HMRC {
  /// <summary>The Api client used to interact with the HMRC API.</summary>
  public class Client {
    #region Fields
    /// <summary>The location of the production API.</summary>
    public const string ProductionUrl = "https://api.service.hmrc.gov.uk";

    /// <summary>The location of the sandbox API.</summary>
    public const string SandboxUrl = "https://test-api.service.hmrc.gov.uk";
    #endregion

    #region Properties
    /// <summary>A cached list of APIs.</summary>
    private List<Api.IApi> Apis = new List<IApi>();

    /// <summary>The short-lived access token.</summary>
    public string AccessToken { get; set; }

    /// <summary>Gets or sets the information used to generate the anti fraud headers.</summary>
    public AntiFraud.AntiFraud AntiFraud { get; set; }

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
    public string ServerToken { get; set; }
    #endregion

    #region APIs
    /// <summary>The Hello World API.</summary>
    public CreateTestUserApi CreateTestUser => GetApi<CreateTestUserApi>();

    /// <summary>The Hello World API.</summary>
    public HelloWorldApi HelloWorld => GetApi<HelloWorldApi>();

    /// <summary>The Hello World API.</summary>
    public TestFraudPreventionApi TestFraudPrevention => GetApi<TestFraudPreventionApi>();

    /// <summary>The VAT API.</summary>
    public VatApi Vat => GetApi<VatApi>();
    #endregion

    #region Constructors
    /// <summary>Creates an instance of the TipsTrade.HMRC.Client class.</summary>
    /// <param name="clientID">The ID used to identify your application during each step of an OAuth 2.0 journey.</param>
    /// <param name="clientSecret">The secret passphrase used to authorise your application during each step of an OAuth 2.0 journey.</param>
    /// <param name="serverToken">The secret token used to authorise your application when making requests to any application-restricted endpoint.</param>
    /// <param name="isSandbox">A flag indicating whether the client is accessing the sandbox environment.</param>
    public Client(string clientID = null, string clientSecret = null, string serverToken = null, bool isSandbox = false) {
      ClientID = clientID;
      ClientSecret = clientSecret;
      ServerToken = serverToken;
      IsSandbox = isSandbox;
    }
    #endregion

    #region Methods
    /// <summary>Gets the specifeid API from the cache.</summary>
    private T GetApi<T>() where T : class, Api.IApi, Api.IClient {
      var found = Apis.Where(x => x is T).FirstOrDefault() as T;
      if (found == null) {
        found = Api.ApiFactory<T>.Create(this);
        Apis.Add(found);
      }
      return found;
    }

    /// <summary>Gets the Uri for the Authorization endpoint.</summary>
    /// <param name="state">
    /// An opaque value used to maintain state between the request and callback and to prevent tampering as described in
    /// the OAuth 2.0 specification (opens in a new tab). This is passed back to your application via the redirect_uri.
    /// </param>
    /// <param name="redirectUrl">	The URI that we use to send users back to your application after successful (or unsuccessful) authorisation.</param>
    /// <param name="scopes">	A list of scopes you would like to have permission to access on behalf of your user.</param>
    /// <returns></returns>
    public string GetAuthorizatoinEndpoint(string state, string redirectUrl, IEnumerable<string> scopes) {
      return GetAuthorizatoinEndpoint(state, redirectUrl, scopes?.ToArray());
    }

    /// <summary>Gets the Uri for the Authorization endpoint.</summary>
    /// <param name="state">
    /// An opaque value used to maintain state between the request and callback and to prevent tampering as described in
    /// the OAuth 2.0 specification (opens in a new tab). This is passed back to your application via the redirect_uri.
    /// </param>
    /// <param name="redirectUrl">	The URI that we use to send users back to your application after successful (or unsuccessful) authorisation.</param>
    /// <param name="scopes">	A list of scopes you would like to have permission to access on behalf of your user.</param>
    /// <returns></returns>
    public string GetAuthorizatoinEndpoint(string state, string redirectUrl, params string[] scopes) {
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

      var request = new RestRequest("oauth/token", Method.POST);
      request.AddParameter("client_secret", ClientSecret);
      request.AddParameter("client_id", ClientID);
      request.AddParameter("grant_type", "authorization_code");
      request.AddParameter("redirect_uri", $"{u.Scheme}://{u.Authority}{u.AbsolutePath}");
      request.AddParameter("code", code);

      var response = restClient.Execute(request);
      response.ThrowOnError();

      var tokens = response.DeserializeContent<TokenResponse>();
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

      var request = new RestRequest("oauth/token", Method.POST);
      request.AddParameter("client_secret", ClientSecret);
      request.AddParameter("client_id", ClientID);
      request.AddParameter("grant_type", "refresh_token");
      request.AddParameter("refresh_token", refreshToken);

      var response = restClient.Execute(request);
      response.ThrowOnError();

      // The OAuth2 flow returns different JSON in the event of an error. Check for that first
      var oauthError = ErrorResponse.FromOAuth2Error(response.Content);
      if (oauthError != null) {
        throw new ApiException(oauthError.Message) {
          ApiError = oauthError,
          Status = response.StatusCode
        };
      }

      var tokens = response.DeserializeContent<TokenResponse>();
      AccessToken = tokens.AccessToken;
      RefreshToken = tokens.RefreshToken;

      return tokens;
    }
    #endregion
  }
}
