using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a response containing a Verified VAT number.</summary>
  public class VerifiedVatNumberCheckResponse : VatNumberCheckResponse {
    /// <summary>The requester of the check.</summary>
    [JsonProperty("requester"), JsonPropertyName("requester")]
    public string Requester { get; set; }

    /// <summary>The consulation number of the request.</summary>
    [JsonProperty("consultationNumber"), JsonPropertyName("consultationNumber")]
    public string ConsultationNumber { get; set; }
  }

  /// <summary>Represents a response containing a checked VAT Number.</summary>
  public class VatNumberCheckResponse {
    /// <summary>The details of the VAT Number.</summary>
    [JsonProperty("target"), JsonPropertyName("target")]
    public VatNumberTarget Target { get; set; }

    /// <summary>The date on which the request was processed.</summary>
    [JsonProperty("processingDate"), JsonPropertyName("processingDate")]
    public DateTime ProcessingDate { get; set; }

    /// <summary></summary>
    public class VatNumberTarget {
      /// <summary>The address of the VAT Number holder.</summary>
      [JsonProperty("address"), JsonPropertyName("address")]
      public Address Address {
        get; set;
      }

      /// <summary>The name of the VAT Number holder.</summary>
      [JsonProperty("name"), JsonPropertyName("name")]
      public string Name { get; set; }

      /// <summary>The address of the VAT Number holder.</summary>
      [JsonProperty("vatNumber"), JsonPropertyName("vatNumber")]
      public string VatNumber { get; set; }
    }

    /// <summary></summary>
    public class Address {
      /// <summary>The first line of the address.</summary>
      [JsonProperty("line1"), JsonPropertyName("line1")]
      public string Line1 { get; set; }

      /// <summary>The postcode of the address.</summary>
      [JsonProperty("postcode"), JsonPropertyName("postcode")]
      public string Postcode { get; set; }

      /// <summary>The country code of the address.</summary>
      [JsonProperty("countryCode"), JsonPropertyName("countryCode")]
      public string CountryCode { get; set; }
    }
  }
}
