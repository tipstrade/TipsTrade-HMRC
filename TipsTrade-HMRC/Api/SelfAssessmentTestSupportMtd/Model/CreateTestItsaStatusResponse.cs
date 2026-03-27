using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model {
  /// <summary>Represents the response when creating and amending a test ITSA status for a specified customer for use within the sandbox environment.</summary>
  public class CreateTestItsaStatusResponse : IApiResponse, ICorrelationId {
    /// <inheritdoc/>
    public Guid CorrelationId { get; set; }
  }
}
