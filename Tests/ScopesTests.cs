using System.Linq;
using NUnit.Framework;

namespace TipsTrade.HMRC.Tests {
  public class ScopesTests : TestBase {
    [Test]
    public void AllScopes() {
      var scopes = Scopes.GetScopes();
      Assert.That(scopes, Is.Not.Empty);
    }

    [Test]
    public void HelloWorld() {
      var scopes = Scopes.GetScopes<Api.HelloWorld.HelloWorldService>();
      Assert.That(scopes, Has.Exactly(1).Items);
    }

    [Test]
    public void SelfAssessment() {
      var expected = new[] {
        Scopes.SelfAssessmentRead,
        Scopes.SelfAssessmentWrite
      };

      var businessDetailsMtd = Scopes.GetScopes<Api.BusinessDetailsMtd.BusinessDetailsMtdService>();
      var obligationsMtd = Scopes.GetScopes<Api.ObligationsMtd.ObligationsMtdService>();
      var selfAssessmentTestSupportMtd = Scopes.GetScopes<Api.SelfAssessmentTestSupportMtd.SelfAssessmentTestSupportMtdService>();
      var selfEmploymentBusinessMtd = Scopes.GetScopes<Api.SelfEmploymentBusinessMtd.SelfEmploymentBusinessMtdService>();

      using (Assert.EnterMultipleScope()) {
        Assert.That(businessDetailsMtd.Count(), Is.EqualTo(2));
        Assert.That(obligationsMtd.Count(), Is.EqualTo(2));
        Assert.That(selfAssessmentTestSupportMtd.Count(), Is.EqualTo(2));
        Assert.That(selfEmploymentBusinessMtd.Count(), Is.EqualTo(2));
      }
    }


    [Test]
    public void Vat() {
      var scopes = Scopes.GetScopes<Api.Vat.VatService>();

      using (Assert.EnterMultipleScope()) {
        Assert.That(scopes.Count(), Is.EqualTo(2));
        Assert.That(scopes, Has.Some.Matches<string>(s => Scopes.VATRead.Equals(s)));
        Assert.That(scopes, Has.Some.Matches<string>(s => Scopes.VATWrite.Equals(s)));
      }
    }
  }
}
