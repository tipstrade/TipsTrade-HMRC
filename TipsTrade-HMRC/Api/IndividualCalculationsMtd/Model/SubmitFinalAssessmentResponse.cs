using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model {
  /// <summary>Represents the response for  submitting a final declaration for a tax year by agreeing to the HMRC's tax calculation.</summary>
  public class SubmitFinalAssessmentResponse : IApiResponse, ICorrelationId, IDeprecationDate, ISunsetDate, IDocumentationLink {
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
