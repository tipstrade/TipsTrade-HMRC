using System;
using TipsTrade.HMRC.Api;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class HelloWorldTests : TestBase {
    public HelloWorldTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void TestApplication() {
      var client = Client;
      Assert.Equal("Hello Application", client.HelloWorld.SayHelloApplication());

      client.ServerToken = $"{Guid.Empty}";
      Assert.Throws<ApiException>(() => client.HelloWorld.SayHelloApplication());
    }

    [Fact]
    public void TestHello() {
      var client = Client;
      Assert.Equal("Hello World", client.HelloWorld.SayHelloWorld());
    }

    [Fact]
    public void TestUser() {
      var client = Client;

      client.AccessToken = AccessToken;
      Assert.Equal("Hello User", client.HelloWorld.SayHelloUser());

      client.AccessToken = $"{Guid.Empty}";
      Assert.Throws<ApiException>(() => client.HelloWorld.SayHelloUser());
    }
  }
}
