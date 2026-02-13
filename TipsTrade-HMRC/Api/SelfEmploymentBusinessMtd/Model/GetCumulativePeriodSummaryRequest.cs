using RestSharp;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model {
  /// <summary>Represents the request for cumulative period income and expenses for a self-employment business that occurred between two dates.</summary>
  public class GetCumulativePeriodSummaryRequest : BaseRequest {
    #region Gov-Test-Scenario constants
    /// <summary>Simulates success response.</summary>
    [GovTestScenario]
    public const string ScenarioDefault = "DEFAULT";

    /// <summary>Simulates success response with consolidatedExpenses.</summary>
    [GovTestScenario]
    public const string ScenarioConsolidatedExpenses = "CONSOLIDATED_EXPENSES";

    /// <summary>Simulates the scenario where no data is found.</summary>
    [GovTestScenario]
    public const string ScenarioNotFound = "NOT_FOUND";

    /// <summary>Simulates the scenario where the tax year is not supported.</summary>
    [GovTestScenario]
    public const string ScenarioTaxYearNotSupported = "TAX_YEAR_NOT_SUPPORTED";

    /// <summary>Performs a stateful retrieve.</summary>
    [GovTestScenario]
    public const string ScenarioStateful = "STATEFUL";
    #endregion

    #region Properties
    #endregion

    #region Implmentations
    /// <inheritdoc/>
    public override Method Method => Method.Get;

    /// <inheritdoc/>
    public override string Location => $"{NiNumber}/{BusinessId}/cumulative/{TaxYear}";

    /// <inheritdoc/>
    public override void PopulateRequest(RestRequest request) {
    }
    #endregion
  }
}
