using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.BusinessDetailsMtd.Model {
  /// <summary>Represents the response for creating or amending quarterly period type for a business.</summary>
  public class AmendQuarterlyPeriodTypeResponse : IApiResponse, ICorrelationId, IDeprecationDate, IDocumentationLink, ISunsetDate {
    /// <inheritdoc/>
    public Guid CorrelationId { get; set; }

    /// <inheritdoc/>
    public string DeprecationDate { get; set; }

    /// <inheritdoc/>
    public string DocumentationLink { get; set; }

    /// <inheritdoc/>
    public string SunsetDate { get; set; }
  }
}
