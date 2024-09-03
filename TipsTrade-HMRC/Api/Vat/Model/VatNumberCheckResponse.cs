using Newtonsoft.Json;
using System;

namespace TipsTrade.HMRC.Api.Vat.Model {
  public class VerifiedVatNumberCheckResponse : VatNumberCheckResponse {
    /// <summary>The requester of the check.</summary>
    [JsonProperty("requester")]
    public string Requester { get; set; }

    /// <summary>The consulation number of the request.</summary>
    [JsonProperty("consultationNumber")]
    public string ConsultationNumber { get; set; }
  }

  /// <summary>Represents a response containing a checked VAT Number.</summary>
  public class VatNumberCheckResponse {
    /// <summary>The details of the VAT Number.</summary>
    [JsonProperty("target")]
    public VatNumberTarget Target { get; set; }

    /// <summary>The date on which the request was processed.</summary>
    [JsonProperty("processingDate")]
    public DateTime ProcessingDate { get; set; }

    /// <summary></summary>
    public class VatNumberTarget {
      /// <summary>The address of the VAT Number holder.</summary>
      [JsonProperty("address")]
      public Address Address {
        get; set;
      }

      /// <summary>The name of the VAT Number holder.</summary>
      [JsonProperty("name")]
      public string Name { get; set; }

      /// <summary>The address of the VAT Number holder.</summary>
      [JsonProperty("vatNumber")]
      public string VatNumber { get; set; }
    }

    /// <summary></summary>
    public class Address {
      /// <summary>The first line of the address.</summary>
      [JsonProperty("line1")]
      public string Line1 { get; set; }

      /// <summary>The postcode of the address.</summary>
      [JsonProperty("postcode")]
      public string Postcode { get; set; }

      /// <summary>The country code of the address.</summary>
      [JsonProperty("countryCode")]
      public string CountryCode { get; set; }
    }
  }
}
