using RestSharp;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model {
  /// <summary>The parameters used to submit a final declaration for a tax year by agreeing to the HMRC's tax calculation.</summary>
  public class SubmitFinalAssessmentRequest : IApiRequest, IGovTestScenario {
    #region Gov-Test-Scenario constants
    /// <summary>Simulate a successful response.</summary>
    [GovTestScenario]
    public const string ScenarioDefault = "DEFAULT";

    /// <summary>Simulates the scenario where the request cannot be completed as it is outside the amendment window.</summary>
    [GovTestScenario]
    public const string ScenarioOutsideAmendmentWindow = "OUTSIDE_AMENDMENT_WINDOW";

    /// <summary>Simulates the scenario where there is a calculation in progress for the tax year.</summary>
    [GovTestScenario]
    public const string ScenarioFinalDeclarationInProgress = "FINAL_DECLARATION_IN_PROGRESS";

    /// <summary>Simulates the scenario where a final declaration has already been received.</summary>
    [GovTestScenario]
    public const string ScenarioFinalDeclarationReceived = "FINAL_DECLARATION_RECEIVED";

    /// <summary>Simulates the scenario where the final declaration cannot be submitted until after the end of the tax year.</summary>
    [GovTestScenario]
    public const string ScenarioFinalDeclarationTaxYear = "FINAL_DECLARATION_TAX_YEAR";

    /// <summary>Simulates the scenario where information relating to an income source has changed.</summary>
    [GovTestScenario]
    public const string ScenarioIncomeSourcesChanged = "INCOME_SOURCES_CHANGED";

    /// <summary>Simulates the scenario where a valid income source cannot be found.</summary>
    [GovTestScenario]
    public const string ScenarioIncomeSourcesInvalid = "INCOME_SOURCES_INVALID";

    /// <summary>Simulates the scenario where no income submissions exist.</summary>
    [GovTestScenario]
    public const string ScenarioNoIncomeSubmissionsExist = "NO_INCOME_SUBMISSIONS_EXIST";

    /// <summary>Simulates the scenario where more recent submissions exist.</summary>
    [GovTestScenario]
    public const string ScenarioRecentSubmissionsExist = "RECENT_SUBMISSIONS_EXIST";

    /// <summary>Simulates the scenario where residency has changed.</summary>
    [GovTestScenario]
    public const string ScenarioResidencyChanged = "RESIDENCY_CHANGED";

    /// <summary>Simulates the scenario where a submission has failed.</summary>
    [GovTestScenario]
    public const string ScenarioSubmissionFailed = "SUBMISSION_FAILED";

    /// <summary>Simulates the scenario where the specified tax year is not supported.</summary>
    [GovTestScenario]
    public const string ScenarioTaxYearNotSupported = "TAX_YEAR_NOT_SUPPORTED";

    /// <summary>Simulates the scenario where the supplied income source could not be found.</summary>
    [GovTestScenario]
    public const string ScenarioNotFound = "NOT_FOUND";
    #endregion

    #region Properties
    /// <summary>
    /// The unique identifier of the calculation.
    /// </summary>
    public string CalculationId { get; set; }

    /// <summary>The optional calculation type requested.</summary>
    /// <remarks>Limited to the following possible values for TY25-26 onwards: "final-declaration" "confirm-amendment"</remarks>
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

    string IApiRequest.Location => $"{NiNumber}/self-assessment/{TaxYear}/{CalculationId}/{CalculationType}";

    /// <inheritdoc/>
    public string GovTestScenario { get; set; }

    void IApiRequest.PopulateRequest(RestRequest request) {
    }
    #endregion
  }
}
