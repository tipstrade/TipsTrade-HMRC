using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.ObligationsMtd.Model {
  /// <summary>Represents a businesses and their obligations details.</summary>
  public class ObligationResult {
    /// <summary>Identifies the type of business income source.</summary>
    /// <remarks>One of "self-employment", "uk-property" or "foreign-property".</remarks>
    [JsonProperty("typeOfBusiness"), JsonPropertyName("typeOfBusiness")]
    public string TypeOfBusiness { get; set; }

    /// <summary>The unique identifier for this business income source.</summary>
    [JsonProperty("businessId"), JsonPropertyName("businessId")]
    public string BusinessId { get; set; }

    /// <summary>An array holding the obligations for the business income source.</summary>
    [JsonProperty("obligationDetails"), JsonPropertyName("obligationDetails")]
    public IEnumerable<ObligationDetail> Obligations { get; set; }
  }
}
