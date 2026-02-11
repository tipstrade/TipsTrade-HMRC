using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.BusinessDetailsMtd.Model {
  /// <summary>Represents the response for business details.</summary>
  public class GetBusinessDetailsResponse : BusinessDetailsResult, IApiResponse, ICorrelationId, IDeprecationDate, IDocumentationLink, ISunsetDate {
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
