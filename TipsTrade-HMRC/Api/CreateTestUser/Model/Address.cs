using Newtonsoft.Json;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>Represents an address.</summary>
  public class Address {
    /// <summary>Organisation's first line of address.</summary>
    [JsonProperty("line1")]
    public string Line1 { get; set; }

    /// <summary>Organisation's second line of address.</summary>
    [JsonProperty("line2")]
    public string Line2 { get; set; }

    /// <summary>Organisation's postcode.</summary>
    [JsonProperty("postcode")]
    public string Postcode { get; set; }

    /// <summary>Returns a string that represents the current object.</summary>
    public override string ToString() {
      return $"{Line1}, {Line2}, {Postcode}";
    }
  }
}
