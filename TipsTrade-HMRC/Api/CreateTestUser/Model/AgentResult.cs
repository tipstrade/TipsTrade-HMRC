using Newtonsoft.Json;
using TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>Represents a response containing a created agent.</summary>
  [RequestType(typeof(CreateAgentRequest))]
  public class AgentResult : UserResultBase {
    /// <summary>Account number for Agent Service.</summary>
    [JsonProperty("agentServicesAccountNumber")]
    public string agentServicesAccountNumber { get; set; }
  }
}
