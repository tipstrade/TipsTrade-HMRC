using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a base response from which all VAT response are derived.</summary>
  public abstract class ResponseBase : ICorrelationId {
    /// <summary>Unique id for operation tracking String, 36 characters.</summary>
    public Guid CorrelationId { get; set; }
  }
}
