using Newtonsoft.Json;
using System;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a result containing a VAT submission.</summary>
  public class SubmitResult {
    /// <summary>The charge reference number is returned, only if the netVatDue value is a debit. Between 1 and 16 characters.</summary>
    [JsonProperty("chargeRefNumber")]
    public string ChargeRefNumber { get; set; }

    /// <summary>Unique number that represents the form bundle. The system stores VAT Return data in forms, which are held in a unique form bundle.</summary>
    [JsonProperty("formBundleNumber")]
    public string FormBundleNumber { get; set; }

    /// <summary>
    /// Is DD if the netVatDue value is a debit and HMRC holds a Direct Debit Instruction for the client.
    /// Is BANK if the netVatDue value is a credit and HMRC holds the client’s bank data. Otherwise not present.
    /// </summary>
    [JsonProperty("paymentIndicator")]
    public string PaymentIndicator { get; set; }

    /// <summary>The time that the message was processed in the system.</summary>
    [JsonProperty("processingDate")]
    public DateTime ProcessingDate { get; set; }
  }
}
