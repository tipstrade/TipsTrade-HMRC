﻿using System;
using TipsTrade.HMRC.Api;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class HelloWorldTests : TestBase {
    public HelloWorldTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void TestApplication() {
      var client = GetClient();
      Assert.Equal("Hello Application", client.HelloWorld.SayHelloApplication());

      client.ServerToken = $"{Guid.Empty}";
      Assert.Throws<ApiException>(() => client.HelloWorld.SayHelloApplication());
    }

    [Fact]
    public void TestHello() {
      var client = GetClient();
      Assert.Equal("Hello World", client.HelloWorld.SayHelloWorld());
    }

    [Fact]
    public void TestUser() {
      var client = GetClient();

      client.AccessToken = Users.Organisation.Tokens.AccessToken;
      Assert.Equal("Hello User", client.HelloWorld.SayHelloUser());

      client.AccessToken = $"{Guid.Empty}";
      Assert.Throws<ApiException>(() => client.HelloWorld.SayHelloUser());
    }
  }
}
