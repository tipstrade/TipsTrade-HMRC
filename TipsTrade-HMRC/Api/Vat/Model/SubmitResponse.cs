using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a response containing a VAT return.</summary>
  public class SubmitResponse : SubmitResult, IApiResponse, ICorrelationId, IReceipt {
    /// <summary>Unique id for operation tracking String, 36 characters.</summary>
    public Guid CorrelationId { get; set; }

    /// <summary>Unique reference number returned for a submission String, 36 characters.</summary>
    public Guid ReceiptID { get; set; }

    /// <summary>The timestamp from the signature.</summary>
    public DateTime ReceiptTimestamp { get; set; }
  }
}
