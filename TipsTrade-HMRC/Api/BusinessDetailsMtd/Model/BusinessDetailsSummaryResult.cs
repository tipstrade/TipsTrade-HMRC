using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.BusinessDetailsMtd.Model {
  /// <summary>Represents a result containing business details summary.</summary>
  public class BusinessDetailsSummaryResult {
    /// <summary>Gets or sets a unique identifier for the business income source. Previously known as selfEmploymentId.</summary>
    /// <remarks>Matches the pattern: ^X[a-zA-Z0-9]{1}IS[0-9]{11}$</remarks>
    [JsonProperty("businessId"), JsonPropertyName("businessId")]
    public string BusinessId { get; set; }

    /// <summary>The trading name of the business.</summary>
    [JsonProperty("tradingName"), JsonPropertyName("tradingName")]
    public string TradingName { get; set; }

    /// <summary>The type of business income source.</summary>
    /// <remarks>Possible values are: self-employment, uk-property, foreign-property, property-unspecified.</remarks>
    [JsonProperty("typeOfBusiness"), JsonPropertyName("typeOfBusiness")]
    public string TypeOfBusiness { get; set; }
  }
}
