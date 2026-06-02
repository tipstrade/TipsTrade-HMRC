using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.HelloWorld;
using TipsTrade.HMRC.Api.Vat;

namespace TipsTrade.HMRC.Tests {
  public class AuthTests : TestBase {
    protected override void CustomSetup() {
      SetupCredentialsForOrganisation();
    }

    [Test]
    public void GetAuthorizationEndpoint_Success() {
      var scopes = new string[] { "hello", "read:vat", "write:vat" };
      var state = $"{Guid.NewGuid()}";
      var redirectUrl = "https://example.com/callback";

      var oAuth = GetOAuthService();
      var actualUrl = oAuth.GetAuthorizationEndpoint(state, redirectUrl, scopes);

      var uri = new Uri(actualUrl);
      var qs = HttpUtility.ParseQueryString(uri.Query);

      Assert.That(qs, Is.Not.Null);
      Assert.That(qs["scope"], Is.Not.Null);

      var actualScopes = qs["scope"].Split(' ');

      Assert.That($"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}", Is.EqualTo($"{HmrcOptions.SandboxUrl}/oauth/authorize"));
      Assert.That(qs["response_type"], Is.EqualTo("code"));
      Assert.That(qs["client_id"], Is.EqualTo(HmrcOptionsMock.Object.Value.ClientID));
      Assert.That(actualScopes, Is.EquivalentTo(scopes));
      Assert.That(qs["state"], Is.EqualTo(state));
      Assert.That(qs["redirect_uri"], Is.EqualTo(redirectUrl));

      TestContext.Out.WriteLine("Authorization Endpoint:");
      TestContext.Out.WriteLine(actualUrl);
    }

    [TestCaseSource(nameof(GetAuthorizationEndpoint_throws_for_Cases))]
    public void GetAuthorizationEndpoint_throws_for(string state, string redirectUrl, string[] scopes, string expectedParamName, Type expectedExceptionType) {
      var oAuth = GetOAuthService();

      Action action = () => oAuth.GetAuthorizationEndpoint(state, redirectUrl, scopes);
      var ex = (ArgumentException?)Assert.Throws(expectedExceptionType, action);

      Assert.That(ex, Is.Not.Null);
      Assert.That(ex.ParamName, Is.EqualTo(expectedParamName));
    }

    [Test]
    public async Task GetApplicationTokenThrows() {
      HmrcOptionsMock.Reset();
      HmrcOptionsMock.Setup(x => x.Value).Returns(() => {
        var options = BuildDefaultOptions();
        options.ClientSecret = "bad-secret";

        return options;
      });

      var svc = GetService<HelloWorldService>();

      // GetApplicationToken is used internally, so the easiest way is to call HelloWorld which will call it if AccessToken isn't set.
      var action = () => svc.SayHelloApplicationAsync();
      var ex = Assert.ThrowsAsync<ApiException>(action);

      using (Assert.EnterMultipleScope()) {
        Assert.That(ex.Message, Is.Not.Null);
        Assert.That(ex.Status, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
      }
    }

    [Test]
    public async Task InvalidCredentials() {
      // Reset and add some bad credentials to the AccessTokenProvider so we can test the handling of invalid credentials.
      AccessTokenProvider.Reset();
      AccessTokenProvider.Setup(m => m.GetCredentialAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Api.Model.TokenResponse() {
        AccessToken = "bad-token",
        RefreshToken = "bad-refresh-token",
        ExpiresIn = 3600,
        ExpiresTimestamp = DateTime.UtcNow.AddHours(1),
        Scope = "hello read:vat write:vat",
        TokenType = "Bearer"
      });

      var request = new Api.Vat.Model.ObligationsRequest() {
        Vrn = "000000000",
        DateFrom = DateTime.Today.AddYears(-1),
        DateTo = DateTime.Today
      };

      var service = GetService<VatService>();
      var action = () => service.GetObligationsAsync(request);

      await Assert.ThatAsync(action, Throws.TypeOf<ApiException>());
    }

    [Test]
    public void HandleRedirectUrlError() {
      var state = $"{Guid.NewGuid()}";
      var uri = $"{RedirectUrl}?error=access_denied&error_description=user+denied+the+authorization&state={state}&error_code=USER_DENIED_AUTHORIZATION";

      var oAuth = GetOAuthService();
      Func<Task> emptyRedirectTask = () => oAuth.HandleEndpointResultAsync(uri, "");
      Func<Task> redirectUrlTask = () => oAuth.HandleEndpointResultAsync(uri, state);

      Assert.ThrowsAsync<InvalidOperationException>(emptyRedirectTask);
      Assert.ThrowsAsync<ApiException>(redirectUrlTask);
    }

    [Test, Ignore("Skipped so the code is one-use only.")]
    public async Task HandleRedirectUrlSuccess() {
      var state = $"{Guid.NewGuid()}";
      var uri = $"{RedirectUrl}?code=640f35efde314a91b32d696710759a5d&state={state}";

      var oAuth = GetOAuthService();
      var tokens = await oAuth.HandleEndpointResultAsync(uri, state);

      TestContext.Out.WriteLine("Token Response:");
      TestContext.Out.WriteLine(JsonConvert.SerializeObject(tokens, Formatting.Indented));

      using (Assert.EnterMultipleScope()) {
        Assert.That(tokens.AccessToken, Is.Not.Null);
        Assert.That(tokens.RefreshToken, Is.Not.Null);
        Assert.That(tokens.ExpiresIn, Is.Not.EqualTo(0));
        AssertExtensions.NotDefault(tokens.ExpiresTimestamp);
        Assert.That(tokens.Scope, Is.Not.Null);
        Assert.That(tokens.TokenType, Is.Not.Null);
      }
    }

    [Test, Ignore("Skipped so we don't accidentally expire our RefreshToken.")]
    //[Test]
    public async Task RefreshToken() {
      var oAuth = GetOAuthService();
      var refreshToken = Users?.Organisation?.Tokens?.RefreshToken ?? throw new InvalidOperationException("RefreshToken is not set for the user.");

      var start = DateTime.UtcNow;
      var expiresSlew = 10; // Allowed slew for the expires
      var tokens = await oAuth.RefreshAccessTokenAsync(refreshToken, default);

      TestContext.Out.WriteLine("Refresh Token Response:");
      TestContext.Out.WriteLine(JsonConvert.SerializeObject(tokens, Formatting.Indented));

      using (Assert.EnterMultipleScope()) {
        Assert.That(tokens.AccessToken, Is.Not.Null);
        Assert.That(tokens.RefreshToken, Is.Not.Null);
        Assert.That(tokens.ExpiresIn, Is.Not.EqualTo(0));
        AssertExtensions.NotDefault(tokens.ExpiresTimestamp);

        Assert.That(tokens.HasAccessTokenExpired(), Is.False); // Using the default slews
        Assert.That(tokens.HasAccessTokenExpired((int)(tokens.ExpiresIn / 60) - Api.Model.TokenResponse.DefaultSlewMinutes), Is.False); // Using 10 minutes before the expected expires
        Assert.That(tokens.HasAccessTokenExpired((int)(tokens.ExpiresIn / 60) + Api.Model.TokenResponse.DefaultSlewMinutes), Is.True); // Using 10 minutes after the expired expires
      }

      var expiresSeconds = tokens.ExpiresTimestamp.Subtract(start).TotalSeconds;
      var expiresDelta = Math.Abs(expiresSeconds - tokens.ExpiresIn);

      using (Assert.EnterMultipleScope()) {
        Assert.That(expiresDelta <= expiresSlew, Is.True);

        Assert.That(tokens.Scope, Is.Not.Null);
        Assert.That(tokens.TokenType, Is.Not.Null);
      }
    }

    private static IEnumerable<TestCaseData> GetAuthorizationEndpoint_throws_for_Cases() {
      var validScopes = new string[] { "hello", "read:vat", "write:vat" };
      var validState = "valid-state";
      var validRedirectUrl = "https://example.com/callback";

      yield return new TestCaseData(null, validRedirectUrl, validScopes, "state", typeof(ArgumentException))
        .SetName("GetAuthorizationEndpoint_throws_for => state=null");

      yield return new TestCaseData(string.Empty, validRedirectUrl, validScopes, "state", typeof(ArgumentException))
        .SetName("GetAuthorizationEndpoint_throws_for => state=empty");

      yield return new TestCaseData(validState, null, validScopes, "redirectUrl", typeof(ArgumentException))
        .SetName("GetAuthorizationEndpoint_throws_for => redirectUrl=null");

      yield return new TestCaseData(validState, string.Empty, validScopes, "redirectUrl", typeof(ArgumentException))
        .SetName("GetAuthorizationEndpoint_throws_for => redirectUrl=empty");

      yield return new TestCaseData(validState, validRedirectUrl, null, "scopes", typeof(ArgumentNullException))
        .SetName("GetAuthorizationEndpoint_throws_for => scopes=null");

      yield return new TestCaseData(validState, validRedirectUrl, new string[0], "scopes", typeof(ArgumentException))
        .SetName("GetAuthorizationEndpoint_throws_for => scopes=empty");
    }
  }
}
