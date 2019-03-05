using Newtonsoft.Json;
using System;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a model that provides a date range.</summary>
  public class DateRange : IDateRange {
    /// <summary>Date from which the range starts.</summary>
    [JsonProperty("from")]
    public DateTime DateFrom { get; set; }

    /// <summary>Date to which the range ends.</summary>
    [JsonProperty("to")]
    public DateTime DateTo { get; set; }
  }
}
