using System;
using System.Linq;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.BusinessDetailsMtd;
using TipsTrade.HMRC.Api.BusinessDetailsMtd.Model;
using TipsTrade.HMRC.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class BusinessDetailsTests : TestBase {
    public BusinessDetailsTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void AmendQuarterlyPeriodType() {
      var svc = GetService<BusinessDetailsMtdService>();

      var resp = svc.CreateOrAmendQuarterlyPeriodType(new AmendQuarterlyPeriodTypeRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessId = "XBIS12345678901", // Self-employment business
        TaxYear = DateTime.Now.GetTaxYear(),
        QuarterlyPeriodType = "calendar",
        GovTestScenario = AmendQuarterlyPeriodTypeRequest.ScenarioDefault
      });

      Assert.NotNull(resp);
    }

    [Fact]
    public void GetBusinessDetails() {
      var svc = GetService<BusinessDetailsMtdService>();

      var resp = svc.GetBusinessDetails(new GetBusinessDetailsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessId = "XBIS12345678901", // Self-employment business
        GovTestScenario = GetBusinessDetailsRequest.ScenarioDefault,
      });

      Assert.NotNull(resp);
      Assert.Equal(TypeOfBusiness.SelfEmployment, resp.TypeOfBusiness);
    }

    [Fact]
    public void GetBusinessDetailsThrows() {
      var svc = GetService<BusinessDetailsMtdService>();

      var ex = Assert.Throws<ApiException>(() => svc.GetBusinessDetails(new GetBusinessDetailsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessId = "XBIS12345678901", // Self-employment business
        GovTestScenario = ListBusinessDetailsRequest.ScenarioNotFound,
      }));
    }

    [Fact]
    public void ListBusinessDetails() {
      var svc = GetService<BusinessDetailsMtdService>();

      var resp = svc.ListBusinessDetails(new ListBusinessDetailsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        GovTestScenario = ListBusinessDetailsRequest.ScenarioBusinessAndProperty
      });

      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);

      var ukProperty = resp.Value.First(x => x.TypeOfBusiness == TypeOfBusiness.UkProperty);
      var foreignProperty = resp.Value.First(x => x.TypeOfBusiness == TypeOfBusiness.ForeignProperty);
      var selfEmployment = resp.Value.First(x => x.TypeOfBusiness == TypeOfBusiness.SelfEmployment);

      Assert.NotNull(ukProperty);
      Assert.NotNull(ukProperty.BusinessId);
      //Assert.NotNull(ukProperty.TradingName); // Can be null

      Assert.NotNull(foreignProperty);
      Assert.NotNull(foreignProperty.BusinessId);
      //Assert.NotNull(foreignProperty.TradingName); // Can be null

      Assert.NotNull(selfEmployment);
      Assert.NotNull(selfEmployment.BusinessId);
      Assert.NotNull(selfEmployment.TradingName);
    }
  }
}
