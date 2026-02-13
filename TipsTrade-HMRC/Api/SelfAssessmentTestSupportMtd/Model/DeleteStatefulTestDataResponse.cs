using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model {
  /// <summary>Represents the response when deleting stateful test data supplied by them in the sandbox environment.</summary>
  public class DeleteStatefulTestDataResponse : IApiResponse, ICorrelationId {
    /// <inheritdoc>
    Guid ICorrelationId.CorrelationId { get; set; }
  }
}
