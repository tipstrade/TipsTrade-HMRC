using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model {
  /// <summary>Represents an ITSA Status.</summary>
  public class ItsaStatusDetails {
    /// <summary>The business income for 2 years prior to the specified taxYear.</summary>
    [JsonProperty("businessIncome2YearsPrior"), JsonPropertyName("businessIncome2YearsPrior")]
    public decimal? BusinessIncome2YearsPrior { get; set; }

    /// <summary>
    /// The ITSA status for the tax year.
    /// </summary>
    /// <remarks>
    /// Enum: "No Status" "MTD Mandated" "MTD Voluntary" "Annual" "Non Digital" "Dormant" "MTD Exempt"
    /// See <see cref="ItsaStatus"/> for more details."
    /// </remarks>
    [JsonProperty("status"), JsonPropertyName("status")]
    public string Status { get; set; }

    /// <summary>
    /// The reason for the ITSA status.
    /// </summary>
    /// <remarks>
    /// Enum: "Sign up - return available" "Sign up - no return available" "ITSA final declaration" "ITSA Q4 declaration" "CESA SA return" "Complex" "Ceased income source" "Reinstated income source"
    /// "Rollover" "Income Source Latency Changes" "MTD ITSA Opt-Out" "MTD ITSA Opt-In" "Digitally Exempt"
    /// See <see cref="ItsaStatusReasons"/> for more details.
    /// </remarks>
    public string StatusReason { get; set; }

    /// <summary>The date the ITSA status was submitted, in the format YYYY-MM-DDThh:mm:ss.SSSZ</summary>
    [JsonProperty("submittedOn"), JsonPropertyName("submittedOn")]
    public string SubmittedOn { get; set; }

    /// <summary>The date the ITSA status was submitted.</summary>
    [Newtonsoft.Json.JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
    public DateTime SubmittedOnDate {
      get => DateTime.TryParse(SubmittedOn, out var date) ? date : default;
      set => SubmittedOn = value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
    }
  }
}
