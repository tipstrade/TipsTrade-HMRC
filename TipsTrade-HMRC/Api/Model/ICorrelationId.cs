using System;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a model that provides provides a correlation ID.</summary>
  public interface ICorrelationId {
    /// <summary>Unique id for operation tracking String, 36 characters.</summary>
    Guid CorrelationId { get; set; }
  }
}
