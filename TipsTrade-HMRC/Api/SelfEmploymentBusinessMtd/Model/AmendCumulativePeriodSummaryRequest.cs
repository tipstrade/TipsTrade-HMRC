using RestSharp;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model {
  /// <summary>Represents the request to submit the cumulative period income and expenses for a self-employment business that occurred between two dates.</summary>
  public class AmendCumulativePeriodSummaryRequest : BaseRequest {
    #region Gov-Test-Scenario constants
    /// <summary>Simulates success response.</summary>
    [GovTestScenario]
    public const string ScenarioDefault = "DEFAULT";

    /// <summary>Simulates the scenario where no data is found.</summary>
    [GovTestScenario]
    public const string ScenarioNotFound = "NOT_FOUND";

    /// <summary>Simulates the scenario where the tax year is not supported.</summary>
    [GovTestScenario]
    public const string ScenarioTaxYearNotSupported = "TAX_YEAR_NOT_SUPPORTED";

    /// <summary>Simulates the scenario where both expenses and consolidatedExpenses are present at the same time.</summary>
    [GovTestScenario]
    public const string ScenarioBothExpensesSupplied = "BOTH_EXPENSES_SUPPLIED";

    /// <summary>Performs a stateful create or update based on a user’s ITSA status and income source. Both standard and calendar updates are supported.</summary>
    [GovTestScenario]
    public const string ScenarioStateful = "STATEFUL";

    /// <summary>Simulates the scenario where Start Date does not align to the commencement date.</summary>
    [GovTestScenario]
    public const string ScenarioStartDateNotAlignedToCommencementDate = "START_DATE_NOT_ALIGNED_TO_COMMENCEMENT_DATE";

    /// <summary>Simulates the scenario where End date does not align to the reporting type.</summary>
    [GovTestScenario]
    public const string ScenarioEndDateNotAlignedWithReportingType = "END_DATE_NOT_ALIGNED_WITH_REPORTING_TYPE";

    /// <summary>Simulates the scenario where Submission start/end date not provided.</summary>
    [GovTestScenario]
    public const string ScenarioMissingSubmissionDates = "MISSING_SUBMISSION_DATES";

    /// <summary>Simulates the scenario where Start/End Date not accepted for annual/latent submission.</summary>
    [GovTestScenario]
    public const string ScenarioStartAndEndDateNotAllowed = "START_AND_END_DATE_NOT_ALLOWED";

    /// <summary>Simulates the scenario where individuals cannot submit data more than 10 days before end of Period.</summary>
    [GovTestScenario]
    public const string ScenarioEarlyDataSubmissionNotAccepted = "EARLY_DATA_SUBMISSION_NOT_ACCEPTED";

    /// <summary>Simulates the scenario where request cannot be completed as it is outside the amendment window.</summary>
    [GovTestScenario]
    public const string ScenarioOutsideAmendmentWindow = "OUTSIDE_AMENDMENT_WINDOW";

    /// <summary>Simulates the scenario for advance submission where the end date must be the end of the period.</summary>
    [GovTestScenario]
    public const string ScenarioAdvanceSubmissionRequiresPeriodEndDate = "ADVANCE_SUBMISSION_REQUIRES_PERIOD_END_DATE";

    /// <summary>Simulates the scenario where the submission end date cannot be earlier than existing submission.</summary>
    [GovTestScenario]
    public const string ScenarioSubmissionEndDateCannotMoveBackwards = "SUBMISSION_END_DATE_CANNOT_MOVE_BACKWARDS";

    /// <summary>Simulates the scenario where the start date does not align with the reporting type.</summary>
    [GovTestScenario]
    public const string ScenarioStartDateNotAlignedWithReportingType = "START_DATE_NOT_ALIGNED_WITH_REPORTING_TYPE";
    #endregion

    #region Properties
    /// <summary>The request payload.</summary>
    public CumulativePeriodSummaryResult Summary { get; set; }
    #endregion

    #region Impementations 
    /// <inheritdoc/>
    public override Method Method => Method.Put;

    /// <inheritdoc/>
    public override string Location => $"{NiNumber}/{BusinessId}/cumulative/{TaxYear}";

    /// <inheritdoc/>
    public override void PopulateRequest(RestRequest request) {
      request.AddJsonBody(Summary);
    }
    #endregion
  }
}
