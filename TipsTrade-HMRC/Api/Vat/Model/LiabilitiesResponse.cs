using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a response containing a VAT liabilities.</summary>
  public class LiabilitiesResponse : IApiListResponse<LiabilityResult>, ICorrelationId {
    /// <summary>Unique id for operation tracking String, 36 characters.</summary>
    public Guid CorrelationId { get; set; }

    /// <summary>The list of VAT liabilities/</summary>
    [JsonProperty("liabilities")]
    public IEnumerable<LiabilityResult> Value { get; set; }
  }
}
