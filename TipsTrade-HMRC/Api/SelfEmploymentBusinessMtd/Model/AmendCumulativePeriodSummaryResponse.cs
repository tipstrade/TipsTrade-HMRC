using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model {
  /// <summary>Represents the respoonse from submitting the cumulative period income and expenses for a self-employment business that occurred between two dates.</summary>
  public class AmendCumulativePeriodSummaryResponse : IApiResponse, ICorrelationId, IDeprecationDate, IDocumentationLink, ISunsetDate {
    #region Implementations
    /// <inheritdoc/>
    public Guid CorrelationId { get; set; }

    /// <inheritdoc/>
    public string DeprecationDate { get; set; }

    /// <inheritdoc/>
    public string DocumentationLink { get; set; }

    /// <inheritdoc/>
    public string SunsetDate { get; set; }
    #endregion
  }
}
