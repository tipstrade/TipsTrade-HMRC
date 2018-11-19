using Newtonsoft.Json;
using System.Collections.Generic;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a response containing a VAT payments.</summary>
  public class PaymentsResponse : ResponseBase, IApiListResponse<PaymentResult> {
    /// <summary>The list of VAT payments.</summary>
    [JsonProperty("payments")]
    public IEnumerable<PaymentResult> Value { get; set; }
  }
}
