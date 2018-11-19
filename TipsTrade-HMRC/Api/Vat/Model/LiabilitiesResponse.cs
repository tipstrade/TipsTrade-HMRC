using Newtonsoft.Json;
using System.Collections.Generic;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a response containing a VAT liabilities.</summary>
  public class LiabilitiesResponse : ResponseBase, IApiListResponse<LiabilityResult> {
    /// <summary>The list of VAT liabilities/</summary>
    [JsonProperty("liabilities")]
    public IEnumerable<LiabilityResult> Value { get; set; }
  }
}
