using System;
using TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model;
using TipsTrade.HMRC.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class IndividualCalculationsMtdTests : TestBase {
    public IndividualCalculationsMtdTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void ListSelfAssessmentCalculations() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var resp = client.IndividualCalculationsMtd.ListSelfAssessmentCalculations(new ListSelfAssessmentCalculationsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = DateTime.Today.GetTaxYear(),
        CalculationType = CalculationType.InYear,
        GovTestScenario = ListSelfAssessmentCalculationsRequest.ScenarioDefault,
      });

      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);
    }

    [Fact]
    public void TriggerSelfAssessmentCalculation() {
      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var resp = client.IndividualCalculationsMtd.TriggerCalculation(new TriggerSelfAssessmentCalculationRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = DateTime.Today.GetTaxYear(),
        CalculationType = CalculationType.InYear,
        GovTestScenario = TriggerSelfAssessmentCalculationRequest.ScenarioDefault,
      });

      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);
    }
  }
}
