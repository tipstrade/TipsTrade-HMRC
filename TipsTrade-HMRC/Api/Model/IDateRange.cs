using System;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a model that provides a date range.</summary>
  public interface IDateRange {
    /// <summary>Date from which to return obligations.</summary>
    DateTime From { get; set; }

    /// <summary>Date to which to return obligations.</summary>
    DateTime To { get; set; }
  }
}
