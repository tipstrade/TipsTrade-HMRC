using TipsTrade.HMRC.Api.OAuth;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class ClientTests : TestBase {
    public ClientTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void Credentials() {
      var options = GetOptions();
      Assert.Equal(ClientId, options.ClientID);
      Assert.Equal(ClientSecret, options.ClientSecret);
      Assert.Equal(IsSandbox, options.IsSandbox);
    }

    [Fact]
    public void Urls() {
      var sandboxOptions = GetOptions();
      sandboxOptions.IsSandbox = true;
      Assert.Equal(HmrcOptions.SandboxUrl, sandboxOptions.BaseUrl);

      var productionOptions = GetOptions();
      productionOptions.IsSandbox = false;
      Assert.Equal(HmrcOptions.ProductionUrl, productionOptions.BaseUrl);
    }
  }
}
