using System;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a model that provides a date range.</summary>
  public interface IDateRange {
    /// <summary>Date from which the range starts.</summary>
    DateTime DateFrom { get; set; }

    /// <summary>Date to which the range ends.</summary>
    DateTime DateTo { get; set; }
  }
}
