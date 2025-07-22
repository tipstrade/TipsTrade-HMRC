using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a result containing a VAT payment.</summary>
  public class PaymentResult {
    /// <summary>The payment value.</summary>
    [JsonProperty("amount"), JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>Payment received date.</summary>
    [JsonProperty("received"), JsonPropertyName("received")]
    public DateTime? Received { get; set; }
  }
}
