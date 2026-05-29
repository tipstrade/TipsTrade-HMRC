using System;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.HelloWorld;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class HelloWorldTests : TestBase {
    public HelloWorldTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void Application() {
      var svc = GetService<HelloWorldService>();
      Assert.Equal("Hello Application", svc.SayHelloApplication());
    }

    [Fact]
    public void Hello() {
      var svc = GetService<HelloWorldService>();
      Assert.Equal("Hello World", svc.SayHelloWorld());
    }

    [Fact]
    public void User() {
      var svc = GetService<HelloWorldService>(Users.Organisation.Tokens.AccessToken);
      Assert.Equal("Hello User", svc.SayHelloUser());

      svc = GetService<HelloWorldService>($"{Guid.Empty}");
      Assert.Throws<ApiException>(() => svc.SayHelloUser());
    }
  }
}
