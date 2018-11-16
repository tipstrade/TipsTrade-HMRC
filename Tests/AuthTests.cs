using Newtonsoft.Json;
using System;
using TipsTrade.HMRC.Api;
using Xunit;

namespace TipsTrade.HMRC.Tests {
  public class AuthTests : TestBase {
    [Fact]
    public void TestEndpointUrl() {
      var client = Client;

      var scopes = new string[] { "hello", "read:vat", "write:vat" };
      var redirecUrl = "https://www.example.com/hmrc/callback";

      Assert.Equal(
        "https://test-api.service.hmrc.gov.uk/oauth/authorize?response_type=code&client_id=7Y7IDapnKX7uGrPhN1SIRe63e1Ya&scope=hello+read%3avat+write%3avat&state=4f00d15e-de25-4796-999f-266ea4429889&redirect_uri=https%3a%2f%2fwww.example.com%2fhmrc%2fcallback",
        client.GetAuthorizatoinEndpoint(State, redirecUrl, scopes));
    }

    [Fact]
    public void TestHandleRedirectUrlError() {
      var uri = "https://www.example.com/hmrc/callback?error=access_denied&error_description=user+denied+the+authorization&state=4f00d15e-de25-4796-999f-266ea4429889&error_code=USER_DENIED_AUTHORIZATION";

      Assert.Throws<InvalidOperationException>(() => Client.HandleEndpointResult(uri, ""));
      Assert.Throws<ApiException>(() => Client.HandleEndpointResult(uri, State));
    }

    [Fact]
    public void TestHandleRedirectUrlSuccess() {
      var uri = "https://www.example.com/hmrc/callback?code=640f35efde314a91b32d696710759a5d&state=4f00d15e-de25-4796-999f-266ea4429889";

      var tokens = Client.HandleEndpointResult(uri, State);
      Assert.NotNull(tokens.AccessToken);
      Assert.NotNull(tokens.RefreshToken);
      Assert.NotEqual(0, tokens.Expires);
      Assert.NotNull(tokens.Scope);
      Assert.NotNull(tokens.TokenType);

      var json = JsonConvert.SerializeObject(tokens);
    }

    [Fact(Skip = "Skipped so we don't accidentally expire our RefreshToken")]
    public void TestRefreshToken() {
      var tokens = Client.RefreshAccessToken(RefreshToken);
      Assert.NotNull(tokens.AccessToken);
      Assert.NotNull(tokens.RefreshToken);
      Assert.NotEqual(0, tokens.Expires);
      Assert.NotNull(tokens.Scope);
      Assert.NotNull(tokens.TokenType);
    }
  }
}
