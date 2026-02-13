using System.Linq;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.BusinessDetailsMtd.Model;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class BusinessDetailsTests : TestBase {
    public BusinessDetailsTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void AmendQuarterlyPeriodType() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var resp = client.BusinessDetailsMtd.CreateOrAmendQuarterlyPeriodType(new AmendQuarterlyPeriodTypeRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessId = "XBIS12345678901", // Self-employment business
        TaxYear = "2023-24",
        QuarterlyPeriodType = "calendar",
        GovTestScenario = AmendQuarterlyPeriodTypeRequest.ScenarioDefault
      });

      Assert.NotNull(resp);
    }

    [Fact]
    public void GetBusinessDetails() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var resp = client.BusinessDetailsMtd.GetBusinessDetails(new GetBusinessDetailsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessId = "XBIS12345678901", // Self-employment business
        GovTestScenario = ListBusinessDetailsRequest.ScenarioDefault,
      });

      Assert.NotNull(resp);
      Assert.Equal(TypeOfBusiness.SelfEmployment, resp.TypeOfBusiness);
    }

    [Fact]
    public void GetBusinessDetailsThrows() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var ex = Assert.Throws<ApiException>(() => client.BusinessDetailsMtd.GetBusinessDetails(new GetBusinessDetailsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessId = "XBIS12345678901", // Self-employment business
        GovTestScenario = ListBusinessDetailsRequest.ScenarioNotFound,
      }));
    }

    [Fact]
    public void ListBusinessDetails() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var resp = client.BusinessDetailsMtd.ListBusinessDetails(new ListBusinessDetailsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        GovTestScenario = ListBusinessDetailsRequest.ScenarioDefault
      });

      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);
      Assert.NotNull(resp.Value.First(x => x.TypeOfBusiness == TypeOfBusiness.SelfEmployment));
    }
  }
}
