using System;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.HelloWorld;
using NUnit.Framework;
using Moq;
using System.Threading;

namespace TipsTrade.HMRC.Tests {
  public class HelloWorldTests : TestBase {
    protected override void CustomSetup() {
      SetupCredentialsForOrganisation();
    }

    [Test]
    public void Application() {
      var svc = GetService<HelloWorldService>();
      Assert.That(svc.SayHelloApplication(), Is.EqualTo("Hello Application"));
    }

    [Test]
    public void Hello() {
      var svc = GetService<HelloWorldService>();
      Assert.That(svc.SayHelloWorld(), Is.EqualTo("Hello World"));
    }

    [Test]
    public void User() {
      var svc = GetService<HelloWorldService>();
      Assert.That(svc.SayHelloUser(), Is.EqualTo("Hello User"));

      // TODO: Fix this
      //svc = GetService<HelloWorldService>($"{Guid.Empty}");
      //Assert.Throws<ApiException>((Action)(() => svc.SayHelloUser()));
    }
  }
}
