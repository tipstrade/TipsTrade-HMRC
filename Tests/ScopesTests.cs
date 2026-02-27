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
    public void SelfAssessment() {
      var expected = new[] {
        Scopes.SelfAssessmentRead,
        Scopes.SelfAssessmentWrite
      };

      var businessDetailsMtd  = Scopes.GetScopes<Api.BusinessDetailsMtd.BusinessDetailsMtdApi>();
      Assert.Equal(2, businessDetailsMtd.Count());

      var obligationsMtd = Scopes.GetScopes<Api.ObligationsMtd.ObligationsMtdApi>();
      Assert.Equal(2, obligationsMtd.Count());

      var selfAssessmentTestSupportMtd = Scopes.GetScopes<Api.SelfAssessmentTestSupportMtd.SelfAssessmentTestSupportMtdApi>();
      Assert.Equal(2, selfAssessmentTestSupportMtd.Count());
     
      var selfEmploymentBusinessMtd = Scopes.GetScopes<Api.SelfEmploymentBusinessMtd.SelfEmploymentBusinessMtdApi>();
      Assert.Equal(2, selfEmploymentBusinessMtd.Count());
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
