using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model {
  /// <summary>Represents the response when creating a test business for use within the Sandbox environment.</summary>
  public class CreateTestBusinessResponse : IApiResponse<string>, ICorrelationId {
    /// <inheritdoc />
    public Guid CorrelationId { get; set; }

    /// <summary>A unique identifier for the business income source.</summary>
    [JsonProperty("businessId"), JsonPropertyName("businessId")]
    public string Value { get; set; }
  }
}
