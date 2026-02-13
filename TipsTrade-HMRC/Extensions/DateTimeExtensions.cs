using System;

namespace TipsTrade.HMRC.Extensions {
  public static class DateTimeExtensions {
    public static string GetTaxYear(this DateTime value) {
      var year = value.GetTaxYearStart().Year;

      return $"{year:0000}-{(year + 1) % 100:00}";
    }

    public static DateTime GetTaxYearEnd(this DateTime value) {
      return value.GetTaxYearStart().AddYears(1).AddDays(-1);
    }

    public static DateTime GetTaxYearStart(this DateTime value) {
      var yearStart = new DateTime(value.Year, 4, 6);

      if (yearStart > value) {
        yearStart = yearStart.AddYears(-1);
      }

      return yearStart;
    }
  }
}
