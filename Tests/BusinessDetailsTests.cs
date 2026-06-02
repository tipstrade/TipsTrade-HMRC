using System;
using System.Linq;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.BusinessDetailsMtd;
using TipsTrade.HMRC.Api.BusinessDetailsMtd.Model;
using TipsTrade.HMRC.Extensions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace TipsTrade.HMRC.Tests {
  public class BusinessDetailsTests : TestBase {
    protected override void CustomSetup() {
      SetupCredentialsForOrganisation();
    }

    private string GetNiNumber() {
      return Users?.Organisation?.User?.NiNumber ?? throw new InvalidOperationException("NiNumber is not set for the user.");
    }

    [Test]
    public async Task AmendQuarterlyPeriodType() {
      var svc = GetService<BusinessDetailsMtdService>();
      var resp = await svc.CreateOrAmendQuarterlyPeriodTypeAsync(new AmendQuarterlyPeriodTypeRequest {
        NiNumber = GetNiNumber(),
        BusinessId = "XBIS12345678901", // Self-employment business
        TaxYear = DateTime.Now.GetTaxYear(),
        QuarterlyPeriodType = "calendar",
        GovTestScenario = AmendQuarterlyPeriodTypeRequest.ScenarioDefault
      });

      Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task GetBusinessDetails() {
      var svc = GetService<BusinessDetailsMtdService>();
      var resp = await svc.GetBusinessDetailsAsync(new GetBusinessDetailsRequest {
        NiNumber = GetNiNumber(),
        BusinessId = "XBIS12345678901", // Self-employment business
        GovTestScenario = GetBusinessDetailsRequest.ScenarioDefault,
      });

      using (Assert.EnterMultipleScope()) {
        Assert.That(resp, Is.Not.Null);
        Assert.That(resp.TypeOfBusiness, Is.EqualTo(TypeOfBusiness.SelfEmployment));
      }
    }

    [Test]
    public void GetBusinessDetailsThrows() {
      var svc = GetService<BusinessDetailsMtdService>();
      var taskToTest = () => svc.GetBusinessDetailsAsync(new GetBusinessDetailsRequest {
        NiNumber = GetNiNumber(),
        BusinessId = "XBIS12345678901", // Self-employment business
        GovTestScenario = ListBusinessDetailsRequest.ScenarioNotFound,
      });

      var ex = Assert.ThrowsAsync<ApiException>(taskToTest);
    }

    [Test]
    public async Task ListBusinessDetails() {
      var svc = GetService<BusinessDetailsMtdService>();
      var resp = await svc.ListBusinessDetailsAsync(new ListBusinessDetailsRequest {
        NiNumber = GetNiNumber(),
        GovTestScenario = ListBusinessDetailsRequest.ScenarioBusinessAndProperty
      });

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);

      var ukProperty = resp.Value.First(x => x.TypeOfBusiness == TypeOfBusiness.UkProperty);
      var foreignProperty = resp.Value.First(x => x.TypeOfBusiness == TypeOfBusiness.ForeignProperty);
      var selfEmployment = resp.Value.First(x => x.TypeOfBusiness == TypeOfBusiness.SelfEmployment);

      using (Assert.EnterMultipleScope()) {
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
}
