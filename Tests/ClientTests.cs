using TipsTrade.HMRC.Api.OAuth;
using NUnit.Framework;

namespace TipsTrade.HMRC.Tests {
  public class ClientTests : TestBase {
    public ClientTests() {
    }

    [SetUp]
    protected override void CustomSetup() {
    }

    [Test]
    public void Credentials() {
      var options = GetOptions();
      Assert.That(options.ClientID, Is.EqualTo(ClientId));
      Assert.That(options.ClientSecret, Is.EqualTo(ClientSecret));
      Assert.That(options.IsSandbox, Is.EqualTo(IsSandbox));
    }

    [Test]
    public void Urls() {
      var sandboxOptions = GetOptions();
      sandboxOptions.IsSandbox = true;
      Assert.That(sandboxOptions.BaseUrl, Is.EqualTo(HmrcOptions.SandboxUrl));

      var productionOptions = GetOptions();
      productionOptions.IsSandbox = false;
      Assert.That(productionOptions.BaseUrl, Is.EqualTo(HmrcOptions.ProductionUrl));
    }
  }
}
