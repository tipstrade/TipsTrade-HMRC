using Newtonsoft.Json;
using System;
using System.Net;
using System.Web;
using TipsTrade.HMRC.Api;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class AuthTests : TestBase {
    public AuthTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void TestEndpointUrl() {
      var client = GetClient();

      var scopes = new string[] { "hello", "read:vat", "write:vat" };

      var encodedRedirect = HttpUtility.UrlEncode(RedirectUrl);
      var expected = $"https://test-api.service.hmrc.gov.uk/oauth/authorize?response_type=code&client_id=7Y7IDapnKX7uGrPhN1SIRe63e1Ya&scope=hello+read%3avat+write%3avat&state=4f00d15e-de25-4796-999f-266ea4429889&redirect_uri={encodedRedirect}";
      var url = client.GetAuthorizatoinEndpoint(State, RedirectUrl, scopes);

      Assert.Equal(expected, url);

      Output.WriteLine("Authorization Endpoint:");
      Output.WriteLine(url);
    }

    [Fact]
    public void TestInvalidCredentials() {
      var client = GetClient();
      ApiException ex;

      var request = new Api.Vat.Model.ObligationsRequest() {
        Vrn = "000000000",
        DateFrom = DateTime.Today.AddYears(-1),
        DateTo = DateTime.Today
      };

      Assert.Throws<InvalidOperationException>(() => client.Vat.GetObligations(request));

      client.AccessToken = Users.Organisation.Tokens.AccessToken;
      ex = Assert.Throws<ApiException>(() => client.Vat.GetObligations(request));

      // The sandbox environment doesn't appear to return the status codes expected.
      //Assert.True(ex.IsInvalidCredentials);
      //Assert.Equal(HttpStatusCode.Unauthorized, ex.Status);
    }

    [Fact]
    public void TestHandleRedirectUrlError() {
      var uri = $"{RedirectUrl}?error=access_denied&error_description=user+denied+the+authorization&state=4f00d15e-de25-4796-999f-266ea4429889&error_code=USER_DENIED_AUTHORIZATION";

      var client = GetClient();

      Assert.Throws<InvalidOperationException>(() => client.HandleEndpointResult(uri, ""));
      Assert.Throws<ApiException>(() => client.HandleEndpointResult(uri, State));
    }

    [Fact(Skip = "Skipped so the code is one-use only.")]
    //[Fact]
    public void TestHandleRedirectUrlSuccess() {
      var uri = $"{RedirectUrl}?code=640f35efde314a91b32d696710759a5d&state=4f00d15e-de25-4796-999f-266ea4429889";

      var client = GetClient();

      var tokens = client.HandleEndpointResult(uri, State);
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
    public void TestRefreshToken() {
      var client = GetClient();

      var tokens = client.RefreshAccessToken(Users.Organisation.Tokens.RefreshToken);
      Assert.NotNull(tokens.AccessToken);
      Assert.NotNull(tokens.RefreshToken);
      Assert.NotEqual(0, tokens.ExpiresIn);
      Assert.NotDefault(tokens.ExpiresTimestamp);
      Assert.NotNull(tokens.Scope);
      Assert.NotNull(tokens.TokenType);

      Output.WriteLine("Refresh Token Response:");
      Output.WriteLine(JsonConvert.SerializeObject(tokens, Formatting.Indented));
    }
  }
}
