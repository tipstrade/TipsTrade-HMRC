using System;
using TipsTrade.HMRC.Api.ObligationsMtd.Model;
using TipsTrade.HMRC.Extensions;
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

      var fromDate = DateTime.Today.GetTaxYearStart();
      var toDate = DateTime.Today.GetTaxYearEnd();

      var resp = client.ObligationsMtd.GetIncomeAndExpenditureObligations(new GetObligationsRequest {
        FromDate = fromDate,
        ToDate = toDate,
        NiNumber = Users.Organisation.User.NiNumber,
        BusinessId = "XBIS12345678901", // Self-employment business
        TypeOfBusiness = TypeOfBusiness.SelfEmployment,
        GovTestScenario = GetObligationsRequest.ScenarioDynamic
      });

      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);
    }
  }
}
