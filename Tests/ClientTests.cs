using System;
using TipsTrade.HMRC;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class ClientTests : TestBase {
    public ClientTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void Credentials() {
      var client = GetClient();
      Assert.Equal(ClientId, client.ClientID);
      Assert.Equal(ClientSecret, client.ClientSecret);
      Assert.Equal(ServerToken, client.ServerToken);
      Assert.Equal(IsSandbox, client.IsSandbox);
    }

    [Fact]
    public void Urls() {
      var client = GetClient();

      client.IsSandbox = true;
      Assert.Equal(Client.SandboxUrl, client.BaseUrl);

      client.IsSandbox = false;
      Assert.Equal(Client.ProductionUrl, client.BaseUrl);
    }
  }
}
