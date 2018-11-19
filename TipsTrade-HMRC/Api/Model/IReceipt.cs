using System;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a model that provides provides a receipt.</summary>
  public interface IReceipt {
    /// <summary>Unique reference number returned for a submission String, 36 characters.</summary>
    Guid ReceiptID { get; set; }

    /// <summary>The timestamp from the signature.</summary>
    DateTime ReceiptTimestamp { get; set; }
  }
}
