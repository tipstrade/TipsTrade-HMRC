using Newtonsoft.Json;
using System;
using TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>Represents a response containing a created individual.</summary>
  [RequestType(typeof(CreateIndividualRequest))]
  public class IndividualResult : UserResultBase {
    /// <summary></summary>
    [JsonProperty("individualDetails")]
    public Individual IndividualDetails { get; set; }

    /// <summary>Self Assessment UTR.</summary>
    [JsonProperty("saUtr")]
    public string SelfAssessmentUtr { get; set; }

    /// <summary>National Insurance number</summary>
    [JsonProperty("nino")]
    public string NiNumber { get; set; }

    /// <summary>Economic Operator Registration and Identification (EORI) number.</summary>
    [JsonProperty("eoriNumber")]
    public string EoriNumber { get; set; }

    /// <summary>Making Tax Digital Income Tax ID</summary>
    [JsonProperty("mtdItId")]
    public string MtdIncomeTaxId { get; set; }

    /// <summary>Represents an individual.</summary>
    public class Individual {
      /// <summary>Individual's first name.</summary>
      [JsonProperty("dateOfBirth")]
      public DateTime DateOfBirth { get; set; }

      /// <summary>Individual's last name.</summary>
      [JsonProperty("firstName")]
      public string FirstName { get; set; }

      /// <summary>Individual's date of birth.</summary>
      [JsonProperty("lastName")]
      public string LastName { get; set; }

      /// <summary>Organisation address.</summary>
      [JsonProperty("address")]
      public Address Address { get; set; }
    }
  }
}
