using RestSharp;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.BusinessDetailsMtd.Model {
  /// <summary>The parameters used to retrieve business details.</summary>
  public class ListBusinessDetailsRequest : IApiRequest, IGovTestScenario {
    /// <summary>Simulate a successful response with a self-employment business.</summary>
    /// <remarks>Use the following businessId for this scenario: XBIS12345678901</remarks>
    [GovTestScenario]
    public const string ScenarioDefault = "DEFAULT";

    /// <summary>Simulate a successful response with a uk-property business.</summary>
    /// <remarks>Use the following businessId for this scenario: XPIS12345678901</remarks>
    [GovTestScenario]
    public const string ScenarioProperty = "PROPERTY";

    /// <summary>Simulate a successful response with a foreign-property business.</summary>
    /// <remarks>Use the following businessId for this scenario: XFIS12345678901</remarks>
    [GovTestScenario]
    public const string ScenarioForeignProperty = "FOREIGN_PROPERTY";

    /// <summary>Simulate a successful response with a property-unspecified business.</summary>
    /// <remarks>Use the following businessId for this scenario: XAIS12345678901</remarks>
    [GovTestScenario]
    public const string ScenarioUnspecified = "UNSPECIFIED";

    /// <summary>Simulate a scenario where no data is found.</summary>
    [GovTestScenario]
    public const string ScenarioNotFound = "NOT_FOUND";

    /// <summary>Returns a dynamic response where the type of response will change corresponding to the businessId provided in the request.</summary>
    /// <remarks>
    /// - For a response with a self-employment business, use businessId: XBIS12345678901
    /// - For a response with a uk-property business, use businessId: XPIS12345678901
    /// - For a response with a foreign-property business, use businessId: XFIS12345678901
    /// - For a response with a property-unspecified business, use businessId: XAIS12345678901
    /// </remarks>
    [GovTestScenario]
    public const string ScenarioDynamic = "DYNAMIC";

    /// <summary>Performs a stateful retrieve.</summary>
    [GovTestScenario]
    public const string ScenarioStateful = "STATEFUL";

    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.Get;

    string IApiRequest.Location => $"{NiNumber}/list";

    /// <summary>National Insurance number, in the format AA999999A.</summary>
    public string NiNumber { get; set; }

    /// <inheritdoc/>
    public string GovTestScenario { get; set; }

    void IApiRequest.PopulateRequest(RestRequest request) {
    }
  }
}
