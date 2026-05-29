using System;
using TipsTrade.HMRC.Api.IndividualCalculationsMtd;
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
      var svc = GetService<IndividualCalculationsMtdService>();

      var resp = svc.ListSelfAssessmentCalculations(new ListSelfAssessmentCalculationsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = DateTime.Today.GetTaxYear(),
        CalculationType = CalculationType.InYear,
        GovTestScenario = ListSelfAssessmentCalculationsRequest.ScenarioDefault,
      });

      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);
    }

    [Fact]
    public void RetrieveSelfAssessmentCalculation() {
      var svc = GetService<IndividualCalculationsMtdService>();

      var taxYear = DateTime.Today.GetTaxYear();

      var resp = svc.RetrieveSelfAssessmentCalculation(new RetrieveSelfAssessmentCalculationRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = taxYear,
        CalculationId = $"{Guid.NewGuid()}",
        GovTestScenario = RetrieveSelfAssessmentCalculationRequest.ScenarioDynamic,
      });

      Assert.NotNull(resp);
      Assert.NotNull(resp.Inputs);
      Assert.NotNull(resp.Metadata);
      Assert.NotNull(resp.Calculation); // Valid for a processed calculation
      Assert.Null(resp.Messages);

      resp = svc.RetrieveSelfAssessmentCalculation(new RetrieveSelfAssessmentCalculationRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = taxYear,
        CalculationId = $"{Guid.NewGuid()}",
        GovTestScenario = RetrieveSelfAssessmentCalculationRequest.ScenarioErrorMessagesExist,
      });

      Assert.NotNull(resp);
      Assert.NotNull(resp.Inputs);
      Assert.NotNull(resp.Metadata);
      Assert.Null(resp.Calculation); // Null for a processed calculation
      Assert.NotNull(resp.Messages);
    }

    [Fact]
    public void SubmitFinalAssessment() {
      var svc = GetService<IndividualCalculationsMtdService>();

      var resp = svc.SubmitFinalAssessment(new SubmitFinalAssessmentRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = DateTime.Today.GetTaxYear(),
        CalculationId = $"{Guid.NewGuid()}",
        CalculationType = CalculationType.FinalDeclaration,
        GovTestScenario = SubmitFinalAssessmentRequest.ScenarioDefault,
      });

      Assert.NotNull(resp);
    }

    [Fact]
    public void TriggerSelfAssessmentCalculation() {
      var svc = GetService<IndividualCalculationsMtdService>();

      var resp = svc.TriggerCalculation(new TriggerSelfAssessmentCalculationRequest {
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
