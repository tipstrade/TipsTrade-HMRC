using TipsTrade.HMRC.Api.ObligationsMtd.Model;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class ObligationsMtdTests : TestBase {
    public ObligationsMtdTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void TestGetFinalObligations() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var resp = client.ObligationsMtd.GetFinalObligations(new GetFinalObligationsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        GovTestScenario = GetFinalObligationsRequest.ScenarioMultiple
      });

      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);
    }

    [Fact]
    public void TestGetObligations() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var resp = client.ObligationsMtd.GetIncomeAndExpenditureObligations(new GetObligationsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessId = "XBIS12345678903", // Self-employment business
        TypeOfBusiness = TypeOfBusiness.SelfEmployment,
        GovTestScenario = GetObligationsRequest.ScenarioOpen
      });

      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);
    }
  }
}
