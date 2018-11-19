using RestSharp;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>The parameters used to retrieve the VAT obligations.</summary>
  public class ObligationsRequest : DateRangeRequest, IApiRequest {
    /// <summary>Simulates the scenario where the client has quarterly obligations and none are fulfilled.</summary>
    [GovTestScenario]
    public const string ScenarioQuarterlyMet0 = "QUARTERLY_NONE_MET";

    /// <summary>Simulates the scenario where the client has quarterly obligations and one is fulfilled.</summary>
    [GovTestScenario]
    public const string ScenarioQuarterlyMet1 = "QUARTERLY_ONE_MET";

    /// <summary>Simulates the scenario where the client has quarterly obligations and two are fulfilled.</summary>
    [GovTestScenario]
    public const string ScenarioQuarterlyMet2 = "QUARTERLY_TWO_MET";

    /// <summary>Simulates the scenario where the client has quarterly obligations and three are fulfilled.</summary>
    [GovTestScenario]
    public const string ScenarioQuarterlyMet3 = "QUARTERLY_THREE_MET";

    /// <summary>Simulates the scenario where the client has quarterly obligations and four are fulfilled.</summary>
    [GovTestScenario]
    public const string ScenarioQuarterlyMet4 = "QUARTERLY_FOUR_MET";

    /// <summary>Simulates the scenario where the client has monthly obligations and none are fulfilled</summary>
    [GovTestScenario]
    public const string ScenarioMonthlylyMet0 = "MONTHLY_NONE_MET";

    /// <summary>Simulates the scenario where the client has monthly obligations and one is fulfilled</summary>
    [GovTestScenario]
    public const string ScenarioMonthlylyMet1 = "MONTHLY_ONE_MET";

    /// <summary>Simulates the scenario where the client has monthly obligations and two are fulfilled</summary>
    [GovTestScenario]
    public const string ScenarioMonthlylyMet2 = "MONTHLY_TWO_MET";

    /// <summary>Simulates the scenario where the client has monthly obligations and three are fulfilled</summary>
    [GovTestScenario]
    public const string ScenarioMonthlylyMet3 = "MONTHLY_THREE_MET";

    /// <summary>Simulates the scenario where no data is found.</summary>
    [GovTestScenario]
    public const string ScenarioNotFound = "NOT_FOUND";

    /// <summary>Which obligation statuses to return (O=Open, F=Fulfilled).</summary>
    public ObligationStatus? Status { get; set; }

    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.GET;

    string IApiRequest.Location => $"{Vrn}/obligations";

    void IApiRequest.PopulateRequest(IRestRequest request) {
      if (Status != null) {
        request.AddParameter("status", $"{Status}"[0]);
      }
    }
  }
}
