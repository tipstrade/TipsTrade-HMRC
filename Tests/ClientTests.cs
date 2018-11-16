using System;
using TipsTrade.HMRC;
using Xunit;

namespace TipsTrade.HMRC.Tests {
  public class ClientTests : TestBase {
    [Fact]
    public void TestCredentials() {
      var client = Client;
      Assert.Equal(ClientId, client.ClientID);
      Assert.Equal(ClientSecret, client.ClientSecret);
      Assert.Equal(ServerToken, client.ServerToken);
      Assert.Equal(IsSandbox, client.IsSandbox);
    }

    [Fact]
    public void TestUrls() {
      var client = Client;

      client.IsSandbox = true;
      Assert.Equal(client.BaseUrl, Client.SandboxUrl);

      client.IsSandbox = false;
      Assert.Equal(client.BaseUrl, Client.ProductionUrl);
    }
  }
}
