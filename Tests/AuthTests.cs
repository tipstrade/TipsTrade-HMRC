using Newtonsoft.Json;
using System;
using System.Web;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.HelloWorld;
using TipsTrade.HMRC.Api.OAuth;
using TipsTrade.HMRC.Api.Vat;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class AuthTests : TestBase {
    public AuthTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void EndpointUrl() {
      var oAuth = GetOAuthService();

      var scopes = new string[] { "hello", "read:vat", "write:vat" };

      var encodedRedirect = HttpUtility.UrlEncode(RedirectUrl);
      var expected = $"https://test-api.service.hmrc.gov.uk/oauth/authorize?response_type=code&client_id=7Y7IDapnKX7uGrPhN1SIRe63e1Ya&scope=hello+read%3avat+write%3avat&state=4f00d15e-de25-4796-999f-266ea4429889&redirect_uri={encodedRedirect}";
      var url = oAuth.GetAuthorizationEndpoint(State, RedirectUrl, scopes);

      Assert.Equal(expected, url);

      Output.WriteLine("Authorization Endpoint:");
      Output.WriteLine(url);
    }

    [Fact]
    public void GetApplicationTokenThrows() {
      var badOptions = GetOptions();
      badOptions.ClientSecret = "bad-secret";
      var badSvc = CreateServiceWithOptions<HelloWorldService>(badOptions);

      // GetApplicationToken is used internally, so the easiest way is to call HelloWorld which will call it if AccessToken isn't set.
      var ex = Assert.Throws<ApiException>(() => badSvc.SayHelloApplication());
      Assert.NotNull(ex.Message);
      Assert.Equal(System.Net.HttpStatusCode.Unauthorized, ex.Status);
    }

    [Fact]
    public void InvalidCredentials() {
      ApiException ex;

      var request = new Api.Vat.Model.ObligationsRequest() {
        Vrn = "000000000",
        DateFrom = DateTime.Today.AddYears(-1),
        DateTo = DateTime.Today
      };

      var svcNoToken = GetService<VatService>();
      Assert.Throws<InvalidOperationException>(() => svcNoToken.GetObligations(request));

      var svcBadToken = GetService<VatService>(Users.Organisation.Tokens.AccessToken);
      ex = Assert.Throws<ApiException>(() => svcBadToken.GetObligations(request));

      // The sandbox environment doesn't appear to return the status codes expected.
      //Assert.True(ex.IsInvalidCredentials);
      //Assert.Equal(HttpStatusCode.Unauthorized, ex.Status);
    }

    [Fact]
    public void HandleRedirectUrlError() {
      var uri = $"{RedirectUrl}?error=access_denied&error_description=user+denied+the+authorization&state=4f00d15e-de25-4796-999f-266ea4429889&error_code=USER_DENIED_AUTHORIZATION";

      var oAuth = GetOAuthService();

      Assert.Throws<InvalidOperationException>(() => oAuth.HandleEndpointResult(uri, ""));
      Assert.Throws<ApiException>(() => oAuth.HandleEndpointResult(uri, State));
    }

    [Fact(Skip = "Skipped so the code is one-use only.")]
    //[Fact]
    public void HandleRedirectUrlSuccess() {
      var uri = $"{RedirectUrl}?code=640f35efde314a91b32d696710759a5d&state=4f00d15e-de25-4796-999f-266ea4429889";

      var oAuth = GetOAuthService();

      var tokens = oAuth.HandleEndpointResult(uri, State);
      Assert.NotNull(tokens.AccessToken);
      Assert.NotNull(tokens.RefreshToken);
      Assert.NotEqual(0, tokens.ExpiresIn);
      Assert.NotDefault(tokens.ExpiresTimestamp);
      Assert.NotNull(tokens.Scope);
      Assert.NotNull(tokens.TokenType);

      Output.WriteLine("Token Response:");
      Output.WriteLine(JsonConvert.SerializeObject(tokens, Formatting.Indented));
    }

    [Fact(Skip = "Skipped so we don't accidentally expire our RefreshToken.")]
    //[Fact]
    public void RefreshToken() {
      var oAuth = GetOAuthService();

      var start = DateTime.UtcNow;
      var expiresSlew = 10; // Allowed slew for the expires

      var tokens = oAuth.RefreshAccessToken(Users.Organisation.Tokens.RefreshToken);
      Assert.NotNull(tokens.AccessToken);
      Assert.NotNull(tokens.RefreshToken);
      Assert.NotEqual(0, tokens.ExpiresIn);
      Assert.NotDefault(tokens.ExpiresTimestamp);

      Assert.False(tokens.HasAccessTokenExpired()); // Using the default slews
      Assert.False(tokens.HasAccessTokenExpired((int)(tokens.ExpiresIn / 60) - Api.Model.TokenResponse.DefaultSlewMinutes)); // Using 10 minutes before the expected expires
      Assert.True(tokens.HasAccessTokenExpired((int)(tokens.ExpiresIn / 60) + Api.Model.TokenResponse.DefaultSlewMinutes)); // Using 10 minutes after the expired expires

      var expiresSeconds = tokens.ExpiresTimestamp.Subtract(start).TotalSeconds;
      var expiresDelta = Math.Abs(expiresSeconds - tokens.ExpiresIn);
      Assert.True(expiresDelta <= expiresSlew);

      Assert.NotNull(tokens.Scope);
      Assert.NotNull(tokens.TokenType);

      Output.WriteLine("Refresh Token Response:");
      Output.WriteLine(JsonConvert.SerializeObject(tokens, Formatting.Indented));
    }
  }
}
