using RestSharp;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.ObligationsMtd.Model {
  /// <summary>The base parameters used to retrieve final declaration (previously known as crystallisation) obligations for a customer’s Income Tax account.</summary>
  public class GetFinalObligationsRequest : BaseRequest {
    #region GovTestScenario Constants
    /// <summary>Simulates a success response with an open obligation.</summary>
    [GovTestScenario]
    public const string ScenarioDefault = "DEFAULT";

    /// <summary>Simulates a success response with multiple obligations.</summary>
    [GovTestScenario]
    public const string ScenarioMultiple = "MULTIPLE";

    /// <summary>Simulates a scenario where the remote endpoint indicates that the trader is insolvent.</summary>
    [GovTestScenario]
    public const string ScenarioInsolventTrader = "INSOLVENT_TRADER";

    /// <summary>Simulates a scenario where no data is found.</summary>
    [GovTestScenario]
    public const string ScenarioNotFound = "NOT_FOUND";

    /// <summary>
    /// The following response values will change to correspond to the values submitted in the request:
    /// <list type="bullet">
    /// <item><description>periodStartDate</description></item>
    /// <item><description>periodEndDate</description></item>
    /// <item><description>dueDate</description></item>
    /// <item><description>receivedDate</description></item>
    /// </list>
    /// </summary>
    [GovTestScenario]
    public const string ScenarioDynamic = "DYNAMIC";
    #endregion

    #region Properties
    /// <summary>The tax year the data applies to.</summary>
    /// <remarks>If a tax year is not specified, returns all obligations starting from 4 years before the current tax year.
    /// and no tax year is specified, all obligations from 2019-20 to 2023-24 are returned.
    /// The earliest allowable tax year is 2017-18.
    /// </remarks>
    public string TaxYear { get; set; }

    /// <summary>Accepted values are ‘open’ and ‘fulfilled’.</summary>
    /// <remarks>If no status is supplied, both open and fulfilled obligations are returned.</remarks>
    public string Status { get; set; }
    #endregion

    #region Overrides
    /// <inheritdoc>
    public override string Location => $"{NiNumber}/crystallisation";

    /// <inheritdoc/>
    public override void PopulateRequest(RestRequest request) {
      if (!string.IsNullOrEmpty(TaxYear)) {
        request.AddQueryParameter("taxYear", TaxYear);
      }
      if (!string.IsNullOrEmpty(Status)) {
        request.AddQueryParameter("status", Status);
      }
    }
    #endregion
  }
}
