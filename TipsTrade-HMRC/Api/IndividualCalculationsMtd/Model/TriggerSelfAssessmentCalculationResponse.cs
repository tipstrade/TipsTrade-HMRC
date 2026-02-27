using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model {
  /// <summary>Represents the response for triggering a self assessment tax calculation for a given tax year.</summary>
  public class TriggerSelfAssessmentCalculationResponse : IApiResponse<string>, ICorrelationId, IDeprecationDate, ISunsetDate, IDocumentationLink {
    /// <inheritdoc/>
    public Guid CorrelationId { get; set; }

    /// <inheritdoc/>
    public string DeprecationDate { get; set; }

    /// <inheritdoc/>
    public string DocumentationLink { get; set; }

    /// <inheritdoc/>
    public string SunsetDate { get; set; }

    /// <summary>The unique identifier of the calculation.</summary>
    [JsonProperty("calculationId"), JsonPropertyName("calculationId")]
    public string Value { get; set; }
  }
}
