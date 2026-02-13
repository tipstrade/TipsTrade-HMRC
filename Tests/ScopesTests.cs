using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class ScopesTests : TestBase {
    public ScopesTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void AllScopes() {
      var scopes = Scopes.GetScopes();
      Assert.NotEmpty(scopes);
    }

    [Fact]
    public void HelloWorld() {
      var scopes = Scopes.GetScopes<Api.HelloWorld.HelloWorldApi>();
      Assert.Single(scopes);
    }

    [Fact]
    public void Vat() {
      var scopes = Scopes.GetScopes<Api.Vat.VatApi>();
      Assert.Equal(2, scopes.Count());
      Assert.Contains(scopes, s => Scopes.VATRead.Equals(s));
      Assert.Contains(scopes, s => Scopes.VATWrite.Equals(s));
    }
  }
}
