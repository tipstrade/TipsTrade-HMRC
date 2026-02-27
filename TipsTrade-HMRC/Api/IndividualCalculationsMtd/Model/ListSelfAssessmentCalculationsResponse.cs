using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model {
  /// <summary>Represents the response for listing Self Assessment tax calculations</summary>
  public class ListSelfAssessmentCalculationsResponse : IApiListResponse<CalculationSummary>, ICorrelationId, IDeprecationDate, ISunsetDate, IDocumentationLink {
    /// <inheritdoc/>
    public Guid CorrelationId { get; set; }

    /// <inheritdoc/>
    public string DeprecationDate { get; set; }

    /// <inheritdoc/>
    public string DocumentationLink { get; set; }

    /// <inheritdoc/>
    public string SunsetDate { get; set; }

    /// <summary>The calculation details.</summary>
    [JsonProperty("calculations"), JsonPropertyName("calculations")]
    public IEnumerable<CalculationSummary> Value { get; set; }
  }
}
