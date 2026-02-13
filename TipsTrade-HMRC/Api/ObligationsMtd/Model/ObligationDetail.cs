using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.ObligationsMtd.Model {
  /// <summary>Represents obligation details.</summary>
  public class ObligationDetail {
    /// <summary>The start date of this obligation period.</summary>
    [JsonProperty("periodStartDate"), JsonPropertyName("periodStartDate")]
    public DateTime PeriodStartDate { get; set; }

    /// <summary>The end date of this obligation period.</summary>
    [JsonProperty("periodEndDate"), JsonPropertyName("periodEndDate")]
    public DateTime PeriodEndDate { get; set; }

    /// <summary>The status of the obligation, one of: open or fulfilled.</summary>
    /// <remarks>Enum: "open" "fulfilled"</remarks>
    [JsonProperty("status"), JsonPropertyName("status")]
    public string Status { get; set; }

    /// <summary>The date this obligation period was fulfilled.</summary>
    [JsonProperty("receivedDate"), JsonPropertyName("receivedDate")]
    public DateTime? ReceivedDate { get; set; }

    /// <inheritdoc/>
    public override string ToString() {
      return $"{PeriodStartDate:yyyy-MM-dd} to {PeriodEndDate:yyyy-MM-dd} ({Status})";
    }
  }
}
