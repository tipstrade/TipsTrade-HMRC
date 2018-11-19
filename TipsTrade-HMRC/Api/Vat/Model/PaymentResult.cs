using Newtonsoft.Json;
using System;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a response containing VAT payment.</summary>
  public class PaymentResult {
    /// <summary>The payment value.</summary>
    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    /// <summary>Payment received date.</summary>
    [JsonProperty("received")]
    public DateTime? Received { get; set; }
  }
}
