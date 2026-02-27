using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model {
  /// <summary>Represents the response for retrieving a Self Assessment tax calculation.</summary>
  public class RetrieveSelfAssessmentCalculationResponse : IApiResponse, ICorrelationId, IDeprecationDate, ISunsetDate, IDocumentationLink {
    /// <inheritdoc/>
    public Guid CorrelationId { get; set; }

    /// <inheritdoc/>
    public string DeprecationDate { get; set; }

    /// <inheritdoc/>
    public string DocumentationLink { get; set; }

    /// <inheritdoc/>
    public string SunsetDate { get; set; }

    /// <summary>Object containing the tax calculation result.</summary>
    [JsonProperty("calculation"), JsonPropertyName("calculation")]
    public object Calculation { get; set; }

    /// <summary>Object containing the input data supplied for use in the retrieved tax calculation.</summary>
    [JsonProperty("inputs"), JsonPropertyName("inputs")]
    public object Inputs { get; set; }

    /// <summary>Object containing messages associated with the retrieved tax calculation.</summary>
    [JsonProperty("messages"), JsonPropertyName("messages")]
    public CalculationMessages Messages { get; set; }

    /// <summary>Object containing top-level metadata information about the retrieved tax calculation.</summary>
    [JsonProperty("metadata"), JsonPropertyName("metadata")]
    public CalculationMetadata Metadata { get; set; }
  }
}