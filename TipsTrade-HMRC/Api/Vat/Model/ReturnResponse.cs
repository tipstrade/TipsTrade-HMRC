using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a response containing a VAT return.</summary>
  public class ReturnResponse : VatReturn, IApiResponse, ICorrelationId {
    /// <summary>Unique id for operation tracking String, 36 characters.</summary>
    public Guid CorrelationId { get; set; }
  }
}
