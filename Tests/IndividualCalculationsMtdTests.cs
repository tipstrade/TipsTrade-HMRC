using System;
using TipsTrade.HMRC.Api.IndividualCalculationsMtd;
using TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model;
using TipsTrade.HMRC.Extensions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace TipsTrade.HMRC.Tests {
  public class IndividualCalculationsMtdTests : TestBase {
    protected override void CustomSetup() {
      SetupCredentialsForOrganisation();
    }

    private string GetNiNumber() {
      return Users?.Organisation?.User?.NiNumber ?? throw new InvalidOperationException("NiNumber is not set for the user.");
    }

    [Test]
    public async Task ListSelfAssessmentCalculations() {
      var svc = GetService<IndividualCalculationsMtdService>();
      var resp = await svc.ListSelfAssessmentCalculationsAsync(new ListSelfAssessmentCalculationsRequest {
        NiNumber = GetNiNumber(),
        TaxYear = DateTime.Today.GetTaxYear(),
        CalculationType = CalculationType.InYear,
        GovTestScenario = ListSelfAssessmentCalculationsRequest.ScenarioDefault,
      });

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);
    }

    [Test]
    public async Task RetrieveSelfAssessmentCalculation() {
      var svc = GetService<IndividualCalculationsMtdService>();
      var taxYear = DateTime.Today.GetTaxYear();
      var resp = await svc.RetrieveSelfAssessmentCalculationAsync(new RetrieveSelfAssessmentCalculationRequest {
        NiNumber = GetNiNumber(),
        TaxYear = taxYear,
        CalculationId = $"{Guid.NewGuid()}",
        GovTestScenario = RetrieveSelfAssessmentCalculationRequest.ScenarioDynamic,
      });

      using (Assert.EnterMultipleScope()) {
        Assert.That(resp, Is.Not.Null);
        Assert.That(resp.Inputs, Is.Not.Null);
        Assert.That(resp.Metadata, Is.Not.Null);
        Assert.That(resp.Calculation, Is.Not.Null); // Valid for a processed calculation
        Assert.That(resp.Messages, Is.Null);
      }

      resp = await svc.RetrieveSelfAssessmentCalculationAsync(new RetrieveSelfAssessmentCalculationRequest {
        NiNumber = GetNiNumber(),
        TaxYear = taxYear,
        CalculationId = $"{Guid.NewGuid()}",
        GovTestScenario = RetrieveSelfAssessmentCalculationRequest.ScenarioErrorMessagesExist,
      });

      using (Assert.EnterMultipleScope()) {
        Assert.That(resp, Is.Not.Null);
        Assert.That(resp.Inputs, Is.Not.Null);
        Assert.That(resp.Metadata, Is.Not.Null);
        Assert.That(resp.Calculation, Is.Null); // Null for a processed calculation
        Assert.That(resp.Messages, Is.Not.Null);
      }
    }

    [Test]
    public async Task SubmitFinalAssessment() {
      var svc = GetService<IndividualCalculationsMtdService>();
      var resp = await svc.SubmitFinalAssessmentAsync(new SubmitFinalAssessmentRequest {
        NiNumber = GetNiNumber(),
        TaxYear = DateTime.Today.GetTaxYear(),
        CalculationId = $"{Guid.NewGuid()}",
        CalculationType = CalculationType.FinalDeclaration,
        GovTestScenario = SubmitFinalAssessmentRequest.ScenarioDefault,
      });

      Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task TriggerSelfAssessmentCalculation() {
      var svc = GetService<IndividualCalculationsMtdService>();
      var resp = await svc.TriggerCalculationAsync(new TriggerSelfAssessmentCalculationRequest {
        NiNumber = GetNiNumber(),
        TaxYear = DateTime.Today.GetTaxYear(),
        CalculationType = CalculationType.InYear,
        GovTestScenario = TriggerSelfAssessmentCalculationRequest.ScenarioDefault,
      });

      using (Assert.EnterMultipleScope()) {
        Assert.That(resp, Is.Not.Null);
        Assert.That(resp.Value, Is.Not.Empty);
      }
    }
  }
}
