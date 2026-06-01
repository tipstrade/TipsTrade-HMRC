using System;
using TipsTrade.HMRC.Api.IndividualCalculationsMtd;
using TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model;
using TipsTrade.HMRC.Extensions;
using NUnit.Framework;

namespace TipsTrade.HMRC.Tests {
  public class IndividualCalculationsMtdTests : TestBase {
    protected override void CustomSetup() {
      SetupCredentialsForOrganisation();
    }

    [Test]
    public void ListSelfAssessmentCalculations() {
      var svc = GetService<IndividualCalculationsMtdService>();

      var resp = svc.ListSelfAssessmentCalculations(new ListSelfAssessmentCalculationsRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = DateTime.Today.GetTaxYear(),
        CalculationType = CalculationType.InYear,
        GovTestScenario = ListSelfAssessmentCalculationsRequest.ScenarioDefault,
      });

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);
    }

    [Test]
    public void RetrieveSelfAssessmentCalculation() {
      var svc = GetService<IndividualCalculationsMtdService>();

      var taxYear = DateTime.Today.GetTaxYear();

      var resp = svc.RetrieveSelfAssessmentCalculation(new RetrieveSelfAssessmentCalculationRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = taxYear,
        CalculationId = $"{Guid.NewGuid()}",
        GovTestScenario = RetrieveSelfAssessmentCalculationRequest.ScenarioDynamic,
      });

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Inputs, Is.Not.Null);
      Assert.That(resp.Metadata, Is.Not.Null);
      Assert.That(resp.Calculation, Is.Not.Null); // Valid for a processed calculation
      Assert.That(resp.Messages, Is.Null);

      resp = svc.RetrieveSelfAssessmentCalculation(new RetrieveSelfAssessmentCalculationRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = taxYear,
        CalculationId = $"{Guid.NewGuid()}",
        GovTestScenario = RetrieveSelfAssessmentCalculationRequest.ScenarioErrorMessagesExist,
      });

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Inputs, Is.Not.Null);
      Assert.That(resp.Metadata, Is.Not.Null);
      Assert.That(resp.Calculation, Is.Null); // Null for a processed calculation
      Assert.That(resp.Messages, Is.Not.Null);
    }

    [Test]
    public void SubmitFinalAssessment() {
      var svc = GetService<IndividualCalculationsMtdService>();

      var resp = svc.SubmitFinalAssessment(new SubmitFinalAssessmentRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = DateTime.Today.GetTaxYear(),
        CalculationId = $"{Guid.NewGuid()}",
        CalculationType = CalculationType.FinalDeclaration,
        GovTestScenario = SubmitFinalAssessmentRequest.ScenarioDefault,
      });

      Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public void TriggerSelfAssessmentCalculation() {
      var svc = GetService<IndividualCalculationsMtdService>();

      var resp = svc.TriggerCalculation(new TriggerSelfAssessmentCalculationRequest {
        NiNumber = Users.Organisation.User.NiNumber,
        TaxYear = DateTime.Today.GetTaxYear(),
        CalculationType = CalculationType.InYear,
        GovTestScenario = TriggerSelfAssessmentCalculationRequest.ScenarioDefault,
      });

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);
    }
  }
}
