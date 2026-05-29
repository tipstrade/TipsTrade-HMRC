using Newtonsoft.Json;
using System;
using System.Web;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.HelloWorld;
using TipsTrade.HMRC.Api.OAuth;
using TipsTrade.HMRC.Api.Vat;
using NUnit.Framework;

namespace TipsTrade.HMRC.Tests {
  public class AuthTests : TestBase {
    public AuthTests() {
    }

    [Test]
    public void EndpointUrl() {
      var oAuth = GetOAuthService();

      var scopes = new string[] { "hello", "read:vat", "write:vat" };

      var encodedRedirect = HttpUtility.UrlEncode(RedirectUrl);
      var expected = $"https://test-api.service.hmrc.gov.uk/oauth/authorize?response_type=code&client_id=7Y7IDapnKX7uGrPhN1SIRe63e1Ya&scope=hello+read%3avat+write%3avat&state=4f00d15e-de25-4796-999f-266ea4429889&redirect_uri={encodedRedirect}";
      var url = oAuth.GetAuthorizationEndpoint(State, RedirectUrl, scopes);

      Assert.That(url, Is.EqualTo(expected));

      TestContext.Progress.WriteLine("Authorization Endpoint:");
      TestContext.Progress.WriteLine(url);
    }

    [Test]
    public void GetApplicationTokenThrows() {
      var badOptions = GetOptions();
      badOptions.ClientSecret = "bad-secret";
      var badSvc = CreateServiceWithOptions<HelloWorldService>(badOptions);

      // GetApplicationToken is used internally, so the easiest way is to call HelloWorld which will call it if AccessToken isn't set.
      var ex = Assert.Throws<ApiException>((Action)(() => badSvc.SayHelloApplication()));
      Assert.That(ex.Message, Is.Not.Null);
      Assert.That(ex.Status, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }

    [Test, Ignore("Cannot test invalid credentials in the sandbox environment.")]
    public void InvalidCredentials() {
      //ApiException ex;

      //var request = new Api.Vat.Model.ObligationsRequest() {
      //  Vrn = "000000000",
      //  DateFrom = DateTime.Today.AddYears(-1),
      //  DateTo = DateTime.Today
      //};

      //var svcNoToken = GetService<VatService>();
      //Assert.Throws<ApiException>((Action)(() => svcNoToken.GetObligations(request)));

      //var svcBadToken = GetService<VatService>();
      //ex = Assert.Throws<ApiException>((Action)(() => svcBadToken.GetObligations(request)));

      //// The sandbox environment doesn't appear to return the status codes expected.
      ////Assert.That(ex.IsInvalidCredentials, Is.True);
      ////Assert.That(ex.Status, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public void HandleRedirectUrlError() {
      var uri = $"{RedirectUrl}?error=access_denied&error_description=user+denied+the+authorization&state=4f00d15e-de25-4796-999f-266ea4429889&error_code=USER_DENIED_AUTHORIZATION";

      var oAuth = GetOAuthService();

      Assert.Throws<InvalidOperationException>((Action)(() => oAuth.HandleEndpointResult(uri, "")));
      Assert.Throws<ApiException>((Action)(() => oAuth.HandleEndpointResult(uri, State)));
    }

    [Test, Ignore("Skipped so the code is one-use only.")]
    //[Test]
    public void HandleRedirectUrlSuccess() {
      var uri = $"{RedirectUrl}?code=640f35efde314a91b32d696710759a5d&state=4f00d15e-de25-4796-999f-266ea4429889";

      var oAuth = GetOAuthService();

      var tokens = oAuth.HandleEndpointResult(uri, State);
      Assert.That(tokens.AccessToken, Is.Not.Null);
      Assert.That(tokens.RefreshToken, Is.Not.Null);
      Assert.That(tokens.ExpiresIn, Is.Not.EqualTo(0));
      AssertExtensions.NotDefault(tokens.ExpiresTimestamp);
      Assert.That(tokens.Scope, Is.Not.Null);
      Assert.That(tokens.TokenType, Is.Not.Null);

      TestContext.Progress.WriteLine("Token Response:");
      TestContext.Progress.WriteLine(JsonConvert.SerializeObject(tokens, Formatting.Indented));
    }

    [Test, Ignore("Skipped so we don't accidentally expire our RefreshToken.")]
    //[Test]
    public void RefreshToken() {
      var oAuth = GetOAuthService();

      var start = DateTime.UtcNow;
      var expiresSlew = 10; // Allowed slew for the expires

      var tokens = oAuth.RefreshAccessTokenAsync(Users.Organisation.Tokens.RefreshToken, default).GetAwaiter().GetResult();
      Assert.That(tokens.AccessToken, Is.Not.Null);
      Assert.That(tokens.RefreshToken, Is.Not.Null);
      Assert.That(tokens.ExpiresIn, Is.Not.EqualTo(0));
      AssertExtensions.NotDefault(tokens.ExpiresTimestamp);

      Assert.False(tokens.HasAccessTokenExpired()); // Using the default slews
      Assert.False(tokens.HasAccessTokenExpired((int)(tokens.ExpiresIn / 60) - Api.Model.TokenResponse.DefaultSlewMinutes)); // Using 10 minutes before the expected expires
      Assert.True(tokens.HasAccessTokenExpired((int)(tokens.ExpiresIn / 60) + Api.Model.TokenResponse.DefaultSlewMinutes)); // Using 10 minutes after the expired expires

      var expiresSeconds = tokens.ExpiresTimestamp.Subtract(start).TotalSeconds;
      var expiresDelta = Math.Abs(expiresSeconds - tokens.ExpiresIn);
      Assert.That(expiresDelta <= expiresSlew, Is.True);

      Assert.That(tokens.Scope, Is.Not.Null);
      Assert.That(tokens.TokenType, Is.Not.Null);

      TestContext.Progress.WriteLine("Refresh Token Response:");
      TestContext.Progress.WriteLine(JsonConvert.SerializeObject(tokens, Formatting.Indented));
    }
  }
}
