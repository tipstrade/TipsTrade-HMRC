using RestSharp;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model {
  /// <summary>The parameters used to trigger a self assessment tax calculation for a given tax year.</summary>
  public class TriggerSelfAssessmentCalculationRequest : IApiRequest, IGovTestScenario {
    #region Gov-Test-Scenario constants
    /// <summary>Simulate a successful response.</summary>
    [GovTestScenario]
    public const string ScenarioDefault = "DEFAULT";

    /// <summary>Simulates the scenario where no income submissions exist for the tax year.</summary>
    [GovTestScenario]
    public const string ScenarioNoIncomeSubmissionsExist = "NO_INCOME_SUBMISSIONS_EXIST";

    /// <summary>Simulates the scenario where a final declaration has already been received.</summary>
    [GovTestScenario]
    public const string ScenarioFinalDeclarationReceived = "FINAL_DECLARATION_RECEIVED";

    /// <summary>Simulates the scenario where income sources data has changed.</summary>
    [GovTestScenario]
    public const string ScenarioIncomeSourcesChanged = "INCOME_SOURCES_CHANGED";

    /// <summary>Simulates the scenario where more recent submissions exist.</summary>
    [GovTestScenario]
    public const string ScenarioRecentSubmissionsExist = "RECENT_SUBMISSIONS_EXIST";

    /// <summary>Simulates the scenario where residency has changed.</summary>
    [GovTestScenario]
    public const string ScenarioResidencyChanged = "RESIDENCY_CHANGED";

    /// <summary>Simulates the scenario where a calculation is in progress.</summary>
    [GovTestScenario]
    public const string ScenarioCalculationInProgress = "CALCULATION_IN_PROGRESS";

    /// <summary>Simulates the scenario where there is a generic business validation rule failure.</summary>
    [GovTestScenario]
    public const string ScenarioBusinessValidationFailure = "BUSINESS_VALIDATION_FAILURE";

    /// <summary>Simulates the scenario where a triggering for a final declaration is performed before the tax year has ended.</summary>
    [GovTestScenario]
    public const string ScenarioTaxYearNotEnded = "TAX_YEAR_NOT_ENDED";
    #endregion

    #region Properties
    /// <summary>The calculation type requested.</summary>
    /// <remarks>Possible values: "in-year", "intent-to-finalise", "intent-to-amend".</remarks>
    public string CalculationType { get; set; }

    /// <summary>National Insurance number, in the format AA999999A.</summary>
    public string NiNumber { get; set; }

    /// <summary>The tax year for which a quarterly period type is being set.</summary>
    /// <remarks>Example: 2023-24</remarks>
    public string TaxYear { get; set; }
    #endregion

    #region Impementations
    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.Post;

    string IApiRequest.Location => $"{NiNumber}/self-assessment/{TaxYear}/trigger/{CalculationType}";

    /// <inheritdoc/>
    public string GovTestScenario { get; set; }

    void IApiRequest.PopulateRequest(RestRequest request) {
    }
    #endregion
  }
}
