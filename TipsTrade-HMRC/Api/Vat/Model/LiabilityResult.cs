using Newtonsoft.Json;
using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a result containing a VAT liability.</summary>
  public class LiabilityResult {
    /// <summary>Liability due date.</summary>
    [JsonProperty("due")]
    public DateTime? Due { get; set; }

    /// <summary>The original liability value.</summary>
    [JsonProperty("originalAmount")]
    public decimal OriginalAmount { get; set; }

    /// <summary>The outstanding liability value.</summary>
    [JsonProperty("outstandingAmount")]
    public decimal? OutstandingAmount { get; set; }

    /// <summary>The Tax period.</summary>
    [JsonProperty("taxPeriod")]
    public DateRange TaxPeriod { get; set; }

    /// <summary>The charge type of this liability. Max length, 30 characters.</summary>
    [JsonProperty("type")]
    public string Type { get; set; }
  }
}
