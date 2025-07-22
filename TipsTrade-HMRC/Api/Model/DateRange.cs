using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a model that provides a date range.</summary>
  public class DateRange : IDateRange {
    /// <summary>Date from which the range starts.</summary>
    [JsonProperty("from"), JsonPropertyName("from")]
    public DateTime DateFrom { get; set; }

    /// <summary>Date to which the range ends.</summary>
    [JsonProperty("to"), JsonPropertyName("to")]
    public DateTime DateTo { get; set; }
  }
}
