using Newtonsoft.Json;
using System.Collections.Generic;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a response containing a VAT obligations.</summary>
  public class ObligationResponse : ResponseBase, IApiListResponse<ObligationsResult> {
    /// <summary>The list of VAT obligations/</summary>
    [JsonProperty("obligations")]
    public IEnumerable<ObligationsResult> Value { get; set; }
  }
}
