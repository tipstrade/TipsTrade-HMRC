using System;
using System.Linq;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.BusinessDetailsMtd;
using TipsTrade.HMRC.Api.BusinessDetailsMtd.Model;
using TipsTrade.HMRC.Extensions;
using NUnit.Framework;

namespace TipsTrade.HMRC.Tests {
  public class BusinessDetailsTests : TestBase {
    protected override void CustomSetup() {
      SetupCredentialsForOrganisation();
    }

    [Test]
    public void AmendQuarterlyPeriodType() {
      var svc = GetService<BusinessDetailsMtdService>();

      var resp = svc.CreateOrAmendQuarterlyPeriodType(new AmendQuarterlyPeriodTypeRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessId = "XBIS12345678901", // Self-employment business
        TaxYear = DateTime.Now.GetTaxYear(),
        QuarterlyPeriodType = "calendar",
        GovTestScenario = AmendQuarterlyPeriodTypeRequest.ScenarioDefault
      });

      Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public void GetBusinessDetails() {
      var svc = GetService<BusinessDetailsMtdService>();

      var resp = svc.GetBusinessDetails(new GetBusinessDetailsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessId = "XBIS12345678901", // Self-employment business
        GovTestScenario = GetBusinessDetailsRequest.ScenarioDefault,
      });

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.TypeOfBusiness, Is.EqualTo(TypeOfBusiness.SelfEmployment));
    }

    [Test]
    public void GetBusinessDetailsThrows() {
      var svc = GetService<BusinessDetailsMtdService>();

      var ex = Assert.Throws<ApiException>((Action)(() => svc.GetBusinessDetails(new GetBusinessDetailsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessId = "XBIS12345678901", // Self-employment business
        GovTestScenario = ListBusinessDetailsRequest.ScenarioNotFound,
      })));
    }

    [Test]
    public void ListBusinessDetails() {
      var svc = GetService<BusinessDetailsMtdService>();

      var resp = svc.ListBusinessDetails(new ListBusinessDetailsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        GovTestScenario = ListBusinessDetailsRequest.ScenarioBusinessAndProperty
      });

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);

      var ukProperty = resp.Value.First(x => x.TypeOfBusiness == TypeOfBusiness.UkProperty);
      var foreignProperty = resp.Value.First(x => x.TypeOfBusiness == TypeOfBusiness.ForeignProperty);
      var selfEmployment = resp.Value.First(x => x.TypeOfBusiness == TypeOfBusiness.SelfEmployment);

      Assert.That(ukProperty, Is.Not.Null);
      Assert.That(ukProperty.BusinessId, Is.Not.Null);
      //Assert.That(ukProperty.TradingName, Is.Not.Null); // Can be null

      Assert.That(foreignProperty, Is.Not.Null);
      Assert.That(foreignProperty.BusinessId, Is.Not.Null);
      //Assert.That(foreignProperty.TradingName, Is.Not.Null); // Can be null

      Assert.That(selfEmployment, Is.Not.Null);
      Assert.That(selfEmployment.BusinessId, Is.Not.Null);
      Assert.That(selfEmployment.TradingName, Is.Not.Null);
    }
  }
}
