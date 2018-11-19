using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a response containing a VAT payments.</summary>
  public class PaymentsResponse : IApiListResponse<PaymentResult>, ICorrelationId {
    /// <summary>Unique id for operation tracking String, 36 characters.</summary>
    public Guid CorrelationId { get; set; }

    /// <summary>The list of VAT payments.</summary>
    [JsonProperty("payments")]
    public IEnumerable<PaymentResult> Value { get; set; }
  }
}
