using TipsTrade.HMRC.Api.HelloWorld;
using NUnit.Framework;
using System.Threading.Tasks;

namespace TipsTrade.HMRC.Tests {
  public class HelloWorldTests : TestBase {
    protected override void CustomSetup() {
      SetupCredentialsForOrganisation();
    }

    [Test]
    public async Task Application() {
      var svc = GetService<HelloWorldService>();
      var resp = await svc.SayHelloApplicationAsync();

      Assert.That(resp, Is.EqualTo("Hello Application"));
    }

    [Test]
    public async Task Hello() {
      var svc = GetService<HelloWorldService>();
      var resp = await svc.SayHelloWorldAsync();

      Assert.That(resp, Is.EqualTo("Hello World"));
    }

    [Test]
    public async Task User() {
      var svc = GetService<HelloWorldService>();
      var resp = await svc.SayHelloUserAsync();

      Assert.That(resp, Is.EqualTo("Hello User"));
    }
  }
}
