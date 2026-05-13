using System;

namespace TipsTrade.HMRC.Extensions {
  /// <summary>
  /// Provides extension methods for <see cref="DateTime"/> to compute UK tax year values.
  /// </summary>
  public static class DateTimeExtensions {
    /// <summary>
    /// Gets the tax year string for the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The date used to determine the tax year.</param>
    /// <returns>
    /// A string in the format "YYYY-YY" representing the tax year that contains <paramref name="value"/>.
    /// For example, a date in the 6 April 2023 through 5 April 2024 tax year would return "2023-24".
    /// </returns>
    public static string GetTaxYear(this DateTime value) {
      var year = value.GetTaxYearStart().Year;

      return $"{year:0000}-{(year + 1) % 100:00}";
    }

    /// <summary>
    /// Gets the inclusive end date of the tax year that contains <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The date used to determine the tax year.</param>
    /// <returns>
    /// A <see cref="DateTime"/> representing the last day of the tax year (the day before the next tax year's start).
    /// The returned value preserves the time component as produced by the underlying arithmetic.
    /// </returns>
    public static DateTime GetTaxYearEnd(this DateTime value) {
      return value.GetTaxYearStart().AddYears(1).AddDays(-1);
    }

    /// <summary>
    /// Gets the start date of the tax year that contains <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The date used to determine the tax year's start.</param>
    /// <returns>
    /// A <see cref="DateTime"/> representing the first day of the tax year (6 April of the tax year's starting calendar year).
    /// </returns>
    /// <remarks>
    /// UK tax years run from 6 April to 5 April of the following calendar year. This method returns 6 April
    /// of the appropriate year for the supplied <paramref name="value"/>. The time portion is set to midnight.
    /// </remarks>
    public static DateTime GetTaxYearStart(this DateTime value) {
      var yearStart = new DateTime(value.Year, 4, 6);

      if (yearStart > value) {
        yearStart = yearStart.AddYears(-1);
      }

      return yearStart;
    }
  }
}
