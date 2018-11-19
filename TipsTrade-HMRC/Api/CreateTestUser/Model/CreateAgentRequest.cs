using Newtonsoft.Json;
using System.Collections.Generic;
using TipsTrade.HMRC.Api.Attributes;
using TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>The parameters used to create an organisation test user.</summary>
  [Endpoint("agents")]
  public class CreateAgentRequest : ICreateTestUserRequest {
    /// <summary>Generates an Account Number for Agent Services and enrols the user for Agent Services.</summary>
    [ServiceName]
    public const string AgentServices = "agent-services";

    /// <summary>The list of services that the user should be enrolled for.</summary>
    [JsonProperty("serviceNames")]
    public List<string> ServiceNames { get; set; } = new List<string>();
  }
}
