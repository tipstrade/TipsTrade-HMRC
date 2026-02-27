using RestSharp;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model {
  /// <summary>The parameters used to list Self Assessment tax calculations for a given National Insurance number and tax year.</summary>
  public class ListSelfAssessmentCalculationsRequest : IApiRequest, IGovTestScenario {
    #region Gov-Test-Scenario constants
    /// <summary>Simulate a successful response.</summary>
    [GovTestScenario]
    public const string ScenarioDefault = "DEFAULT";

    /// <summary>Simulates the scenario where no data is found.</summary>
    [GovTestScenario]
    public const string ScenarioNotFound = "NOT_FOUND";

    /// <summary>
    /// The following response parameters will be based on the data submitted in the request:
    /// <list type="bullet">
    /// <item><description>taxYear</description></item>
    /// <item><description>calculationTimestamp</description></item>
    /// <item><description>finalDeclarationTimestamp</description></item>
    /// </list>
    /// </summary>
    [GovTestScenario]
    public const string ScenarioDynamic = "DYNAMIC";
    #endregion

    #region Properties
    /// <summary>The optional calculation type requested.</summary>
    /// <remarks>Possible values: "in-year", "intent-to-finalise", "intent-to-amend", "final-declaration", "confirm-amendment".</remarks>
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

    Method IApiRequest.Method => Method.Get;

    string IApiRequest.Location => $"{NiNumber}/self-assessment/{TaxYear}";

    /// <inheritdoc/>
    public string GovTestScenario { get; set; }

    void IApiRequest.PopulateRequest(RestRequest request) {
      if (!string.IsNullOrEmpty(CalculationType)) {
        request.AddQueryParameter("calculationType", CalculationType);
      }
    }
    #endregion
  }
}
