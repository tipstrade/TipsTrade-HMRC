using NUnit.Framework;
using System.Threading.Tasks;

namespace TipsTrade.HMRC.Tests {
  public class ClientTests : TestBase {
    [Test]
    public async Task Credentials() {
      var options = await GetOptionsAsync();
      Assert.That(options.ClientId, Is.EqualTo(ClientId));
      Assert.That(options.ClientSecret, Is.EqualTo(ClientSecret));
      Assert.That(options.IsSandbox, Is.EqualTo(IsSandbox));
    }

    [Test]
    public async Task Urls() {
      var sandboxOptions = await GetOptionsAsync();
      sandboxOptions.IsSandbox = true;
      Assert.That(sandboxOptions.BaseUrl, Is.EqualTo(HmrcOptions.SandboxUrl));

      var productionOptions = await GetOptionsAsync();
      productionOptions.IsSandbox = false;
      Assert.That(productionOptions.BaseUrl, Is.EqualTo(HmrcOptions.ProductionUrl));
    }
  }
}
