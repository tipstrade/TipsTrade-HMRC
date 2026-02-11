using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.BusinessDetailsMtd.Model {
  /// <summary>Represents the response for listing business details.</summary>
  public class ListBusinessDetailsResponse : IApiListResponse<BusinessDetailsSummaryResult>, ICorrelationId, IDeprecationDate, IDocumentationLink, ISunsetDate {
    /// <inheritdoc/>
    public Guid CorrelationId { get; set; }

    /// <inheritdoc/>
    public string DeprecationDate { get; set; }

    /// <inheritdoc/>
    public string DocumentationLink { get; set; }

    /// <inheritdoc/>
    public string SunsetDate { get; set; }

    /// <inheritdoc/>
    [JsonProperty("listOfBusinesses"), JsonPropertyName("listOfBusinesses")]
    public IEnumerable<BusinessDetailsSummaryResult> Value { get; set; }
  }
}
