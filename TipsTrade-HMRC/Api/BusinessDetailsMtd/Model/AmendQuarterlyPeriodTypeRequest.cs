using RestSharp;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.BusinessDetailsMtd.Model {
  /// <summary>The parameters used to create or amend quarterly period type for a business.</summary>
  public class AmendQuarterlyPeriodTypeRequest : IApiRequest, IGovTestScenario {
    #region Gov-Test-Scenario constants
    /// <summary>Simulate a successful response.</summary>
    [GovTestScenario]
    public const string ScenarioDefault = "DEFAULT";

    /// <summary>Simulates the scenario in which the business ID is not found.</summary>
    [GovTestScenario]
    public const string ScenarioBusinessIdNotFound = "BUSINESS_ID_NOT_FOUND";

    /// <summary>Simulates the scenario in which the request conflicts with the current state of the business ID.</summary>
    [GovTestScenario]
    public const string ScenarioBusinessIdStateConflict = "BUSINESS_ID_STATE_CONFLICT";

    /// <summary>Simulates the scenario in which the quarterly period type cannot be changed when a business commencement date falls between 1st and 5th of April, and the current date is also within that date range but later than the business commencement date.</summary>
    [GovTestScenario]
    public const string ScenarioQuarterlyPeriodUpdating = "QUARTERLY_PERIOD_UPDATING";

    /// <summary>Performs a stateful create and update.</summary>
    [GovTestScenario]
    public const string ScenarioStateful = "STATEFUL";
    #endregion

    #region Properties
    /// <summary>A unique identifier for the business income source.</summary>
    /// <remarks>It must conform to the following regex: ^X[A-Z0-9]{1}IS[0-9]{11}$</remarks>
    public string BusinessId { get; set; }

    /// <summary>National Insurance number, in the format AA999999A.</summary>
    public string NiNumber { get; set; }

    /// <summary>The tax year for which a quarterly period type is being set.</summary>
    /// <remarks>Example: 2023-24</remarks>
    public string TaxYear { get; set; }

    /// <summary>The quarterly period type that is being set for the business id.</summary>
    /// <remarks>Possible values: "standard", "calendar".</remarks>
    public string QuarterlyPeriodType { get; set; }
    #endregion

    #region Impementations
    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.Put;

    string IApiRequest.Location => $"{NiNumber}/{BusinessId}/{TaxYear}";

    /// <inheritdoc/>
    public string GovTestScenario { get; set; }

    void IApiRequest.PopulateRequest(RestRequest request) {
      request.AddJsonBody(new {
        quarterlyPeriodType = QuarterlyPeriodType
      });
    }
    #endregion
  }
}
