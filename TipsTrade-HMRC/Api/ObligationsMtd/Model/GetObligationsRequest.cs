using RestSharp;
using System;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.ObligationsMtd.Model {
  /// <summary>The base parameters used to retrieve Income Tax (Self Assessment) Income and Expenditure Obligations.</summary>
  public class GetObligationsRequest : BaseRequest {
    #region GovTestScenario Constants
    /// <summary>Simulates a success response.</summary>
    [GovTestScenario]
    public const string ScenarioDefault = "DEFAULT";

    /// <summary>
    /// Simulates a success response with an open obligation.
    /// </summary>
    /// <remarks>
    /// For a response with a self-employment business, use businessId: XBIS12345678903.
    /// For a response with a UK property business, use businessId: XPIS12345678903.
    /// For a response with a foreign property business, use businessId: XFIS12345678903.
    /// </remarks>
    [GovTestScenario]
    public const string ScenarioOpen = "OPEN";

    /// <summary>
    /// Simulates a success response with a fulfilled obligation.
    /// </summary>
    /// <remarks>
    /// For a response with a self-employment business, use businessId: XBIS12345678902.
    /// For a response with a UK property business, use businessId: XPIS12345678902.
    /// For a response with a foreign property business, use businessId: XFIS12345678902.
    /// </remarks>
    [GovTestScenario]
    public const string ScenarioFulfilled = "Fulfilled";

    /// <summary>Simulates a scenario where the remote endpoint indicates that the trader is insolvent.</summary>
    [GovTestScenario]
    public const string ScenarioInsolventTrader = "INSOLVENT_TRADER";

    /// <summary>Simulates a scenario where no data is found.</summary>
    [GovTestScenario]
    public const string ScenarioNotFound = "NOT_FOUND";

    /// <summary>Simulates a scenario where no obligations are found.</summary>
    [GovTestScenario]
    public const string ScenarioNoObligationsFound = "NO_OBLIGATIONS_FOUND";

    /// <summary>
    /// The following response values will change to correspond to the values submitted in the request:
    /// </summary>
    /// <remarks></remarks>
    /// fromDate, toDate, status (open or fulfilled)
    /// 
    /// For a response with a self-employment business, use businessId:XBIS12345678901.
    /// For a response with a UK property business, use businessId: XPIS12345678901.
    /// For a response with a foreign property business, use businessId: XFIS12345678901.
    [GovTestScenario]
    public const string ScenarioDynamic = "DYNAMIC";

    /// <summary>Simulates a success response with cumulative quarterly updates.</summary>
    [GovTestScenario]
    public const string ScenarioCumulative = "CUMULATIVE";
    #endregion

    #region Properties
    /// <summary>The type of business whose obligations are to be returned. If the type is not specified the default is to return obligations for all businesses. The type must be provided if "businessId" is provided.</summary>
    /// <remarks>Enum: "self-employment" "uk-property" "foreign-property"</remarks>
    public string TypeOfBusiness { get; set; }

    /// <summary>The unique identifier for the business whose obligations are to be returned.</summary>
    public string BusinessId { get; set; }

    /// <summary>The start date of the range to filter obligations.</summary>
    /// <remarks>Mandatory if the “to” query parameter is supplied. If the “from” and “to” date parameters are not supplied, the date range will default to a year from today unless the status parameter is set to "open".</remarks>
    public DateTime? FromDate { get; set; }

    /// <summary>The end date of the range to filter obligations.</summary>
    /// <remarks>Mandatory if the “from” query parameter is supplied. If the “from” and “to” date parameters are not supplied, the date range will default to a year from today unless the status parameter is set to "open".</remarks>
    public DateTime? ToDate { get; set; }

    /// <summary>Accepted values are ‘open’ and ‘fulfilled’.</summary>
    /// <remarks>If no status is supplied, both open and fulfilled obligations are returned.</remarks>
    public string Status { get; set; }
    #endregion

    #region Overrides
    /// <inheritdoc>
    public override string Location => $"{NiNumber}/income-and-expenditure";

    /// <inheritdoc/>
    public override void PopulateRequest(RestRequest request) {
      if (!string.IsNullOrEmpty(TypeOfBusiness)) {
        request.AddQueryParameter("typeOfBusiness", TypeOfBusiness);
      }
      if (!string.IsNullOrEmpty(BusinessId)) {
        request.AddQueryParameter("businessId", BusinessId);
      }
      if (FromDate != null) {
        request.AddQueryParameter("fromDate", $"{FromDate:yyyy-MM-dd}");
      }
      if (ToDate != null) {
        request.AddQueryParameter("toDate", $"{ToDate:yyyy-MM-dd}");
      }
      if (!string.IsNullOrEmpty(Status)) {
        request.AddQueryParameter("status", Status);
      }
    }
    #endregion
  }
}
