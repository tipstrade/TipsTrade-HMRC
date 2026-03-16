using RestSharp;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.BusinessDetailsMtd.Model {
  /// <summary>The parameters used to retrieve business details.</summary>
  public class GetBusinessDetailsRequest : IApiRequest, IGovTestScenario {
    #region Gov-Test-Scenario constants
    /// <summary>
    /// Simulate a successful response with a self-employment business.
    /// <list type="bullet">
    /// <item><description>Use the following businessId for this scenario: XBIS12345678901</description></item>
    /// </list>
    /// </summary>
    [GovTestScenario]
    public const string ScenarioDefault = "DEFAULT";

    /// <summary>
    /// Simulate a successful response with a uk-property business.
    /// <list type="bullet">
    /// <item><description>Use the following businessId for this scenario: XPIS12345678901</description></item>
    /// </list> 
    /// </summary>
    [GovTestScenario]
    public const string ScenarioProperty = "PROPERTY";

    /// <summary>
    /// Simulate a successful response with a foreign-property business.
    /// <list type="bullet">
    /// <item><description>For a response with a self-employment business, use businessId: XBIS12345678901</description></item>
    /// </list>
    /// </summary>
    [GovTestScenario]
    public const string ScenarioForeignProperty = "FOREIGN_PROPERTY";

    /// <summary>
    /// Simulate a successful response with a property-unspecified business.
    /// <list type="bullet">
    /// <item><description>Use the following businessId for this scenario: XAIS12345678901</description></item>
    /// </list>
    /// </summary>
    [GovTestScenario]
    public const string ScenarioUnspecified = "UNSPECIFIED";

    /// <summary>Simulate a scenario where no data is found.</summary>
    /// <remarks>No data will be returned for this scenario.</remarks>
    [GovTestScenario]
    public const string ScenarioNotFound = "NOT_FOUND";

    /// <summary>
    /// Returns a dynamic response where the type of response will change corresponding to the businessId provided in the request.
    /// <list type="bullet">
    /// <item><description>For a response with a self-employment business, use businessId: XBIS12345678901</description></item>
    /// <item><description>For a response with a uk-property business, use businessId: XPIS12345678901</description></item>
    /// <item><description>For a response with a foreign-property business, use businessId: XFIS12345678901</description></item>
    /// <item><description>For a response with a property-unspecified business, use businessId: XAIS12345678901</description></item>
    /// </list>
    /// </summary>
    [GovTestScenario]
    public const string ScenarioDynamic = "DYNAMIC";

    /// <summary>Performs a stateful retrieve.</summary>
    [GovTestScenario]
    public const string ScenarioStateful = "STATEFUL";
    #endregion

    #region Properties
    /// <summary>A unique identifier for the business income source.</summary>
    /// <remarks>It must conform to the following regex: ^X[A-Z0-9]{1}IS[0-9]{11}$</remarks>
    public string BusinessId { get; set; }

    /// <summary>National Insurance number, in the format AA999999A.</summary>
    public string NiNumber { get; set; }
    #endregion


    #region Impementations
    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.Get;

    string IApiRequest.Location => $"{NiNumber}/{BusinessId}";

    /// <inheritdoc/>
    public string GovTestScenario { get; set; }

    void IApiRequest.PopulateRequest(RestRequest request) {
    }
    #endregion
  }
}
