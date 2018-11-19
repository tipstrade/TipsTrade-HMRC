using Newtonsoft.Json;
using System;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a model that provides a date range.</summary>
  public class DateRange : IDateRange {
    /// <summary>Date from which to return obligations.</summary>
    [JsonProperty("from")]
    public DateTime From { get; set; }

    /// <summary>Date to which to return obligations.</summary>
    [JsonProperty("to")]
    public DateTime To { get; set; }
  }
}
