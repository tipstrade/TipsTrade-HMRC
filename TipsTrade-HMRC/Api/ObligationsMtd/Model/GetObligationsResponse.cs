using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.ObligationsMtd.Model {
  /// <summary>Represents the response for businesses and their obligations details.</summary>
  public class GetObligationsResponse : IApiListResponse<ObligationResult>, ICorrelationId, IDeprecationDate, IDocumentationLink, ISunsetDate {
    /// <inheritdoc/>
    public Guid CorrelationId { get; set; }

    /// <inheritdoc/>
    public string DeprecationDate { get; set; }

    /// <inheritdoc/>
    public string DocumentationLink { get; set; }

    /// <inheritdoc/>
    public string SunsetDate { get; set; }

    /// <inheritdoc/>
    [JsonProperty("obligations"), JsonPropertyName("obligations")]
    public IEnumerable<ObligationResult> Value { get; set; }
  }
}
