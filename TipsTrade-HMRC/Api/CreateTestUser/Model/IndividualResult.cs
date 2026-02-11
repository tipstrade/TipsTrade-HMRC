using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>Represents a response containing a created individual.</summary>
  [RequestType(typeof(CreateIndividualRequest))]
  public class IndividualResult : UserResultBase {
    /// <summary></summary>
    [JsonProperty("individualDetails"), JsonPropertyName("individualDetails")]
    public Individual IndividualDetails { get; set; }

    /// <summary>Self Assessment UTR.</summary>
    [JsonProperty("saUtr"), JsonPropertyName("saUtr")]
    public string SelfAssessmentUtr { get; set; }

    /// <summary>National Insurance number</summary>
    [JsonProperty("nino"), JsonPropertyName("nino")]
    public string NiNumber { get; set; }

    /// <summary>Economic Operator Registration and Identification (EORI) number.</summary>
    [JsonProperty("eoriNumber"), JsonPropertyName("eoriNumber")]
    public string EoriNumber { get; set; }

    /// <summary>Making Tax Digital Income Tax ID</summary>
    [JsonProperty("mtdItId"), JsonPropertyName("mtdItId")]
    public string MtdIncomeTaxId { get; set; }

    /// <summary>Date of registration for VAT.</summary>
    [JsonProperty("vatRegistrationDate"), JsonPropertyName("vatRegistrationDate")]
    public DateTime VatRegistrationDate { get; set; }

    /// <summary>VAT Reference Number</summary>
    [JsonProperty("vrn"), JsonPropertyName("vrn")]
    public string Vrn { get; set; }

    /// <summary>Represents an individual.</summary>
    public class Individual {
      /// <summary>Individual's first name.</summary>
      [JsonProperty("dateOfBirth"), JsonPropertyName("dateOfBirth")]
      public DateTime DateOfBirth { get; set; }

      /// <summary>Individual's last name.</summary>
      [JsonProperty("firstName"), JsonPropertyName("firstName")]
      public string FirstName { get; set; }

      /// <summary>Individual's date of birth.</summary>
      [JsonProperty("lastName"), JsonPropertyName("lastName")]
      public string LastName { get; set; }

      /// <summary>Organisation address.</summary>
      [JsonProperty("address"), JsonPropertyName("address")  ]
      public Address Address { get; set; }

      /// <summary>Returns a string that represents the current object.</summary>
      public override string ToString() {
        return $"{FirstName} {LastName}";
      }
    }
  }
}
