using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>The parameters used to create an organisation test user.</summary>
  public class CreateAgentRequest : IApiRequest, ICreateTestUserRequest {
    /// <summary>Generates an Account Number for Agent Services and enrols the user for Agent Services.</summary>
    [ServiceName]
    public const string AgentServices = "agent-services";

    /// <summary>The list of services that the user should be enrolled for.</summary>
    [JsonProperty("serviceNames"), JsonPropertyName("serviceNames")]
    public List<string> ServiceNames { get; set; } = new List<string>();

    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.Application;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.Post;

    string IApiRequest.Location => "agents";

    void IApiRequest.PopulateRequest(RestRequest request) {
      request.AddJsonBody(this);
    }
  }
}
