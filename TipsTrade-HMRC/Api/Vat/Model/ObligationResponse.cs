using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a response containing a VAT obligations.</summary>
  public class ObligationResponse : IApiListResponse<ObligationsResult>, ICorrelationId {
    /// <summary>Unique id for operation tracking String, 36 characters.</summary>
    public Guid CorrelationId { get; set; }

    /// <summary>The list of VAT obligations/</summary>
    [JsonProperty("obligations")]
    public IEnumerable<ObligationsResult> Value { get; set; }
  }
}
