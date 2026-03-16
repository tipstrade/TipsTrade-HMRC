using RestSharp;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.BusinessDetailsMtd.Model {
  /// <summary>The parameters used to retrieve business details.</summary>
  public class ListBusinessDetailsRequest : IApiRequest, IGovTestScenario {
    #region Gov-Test-Scenario constants
    /// <summary>Simulate a successful response with a self-employment business.</summary>
    [GovTestScenario]
    public const string ScenarioDefault = "DEFAULT";

    /// <summary>Simulate a successful response with a uk-property business.</summary>
    [GovTestScenario]
    public const string ScenarioProperty = "PROPERTY";

    /// <summary>Simulate a successful response with a foreign-property business.</summary>
    [GovTestScenario]
    public const string ScenarioForeignProperty = "FOREIGN_PROPERTY";

    /// <summary>Simulate a successful response with a self-employment, uk-property and foreign-property business.</summary>
    [GovTestScenario]
    public const string ScenarioBusinessAndProperty = "BUSINESS_AND_PROPERTY";

    /// <summary>Simulate a successful response with a property-unspecified business.</summary>
    [GovTestScenario]
    public const string ScenarioUnspecified = "UNSPECIFIED";

    /// <summary>Simulates the scenario where no data is found.</summary>
    [GovTestScenario]
    public const string ScenarioNotFound = "NOT_FOUND";

    /// <summary>Performs a stateful list.</summary>
    [GovTestScenario]
    public const string ScenarioStateful = "STATEFUL";
    #endregion

    #region Properties
    /// <summary>National Insurance number, in the format AA999999A.</summary>
    public string NiNumber { get; set; }
    #endregion

    #region Implentations
    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.Get;

    string IApiRequest.Location => $"{NiNumber}/list";

    /// <inheritdoc/>
    public string GovTestScenario { get; set; }

    void IApiRequest.PopulateRequest(RestRequest request) {
    }
    #endregion
  }
}
