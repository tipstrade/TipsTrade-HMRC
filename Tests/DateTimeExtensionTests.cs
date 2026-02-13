using System;
using TipsTrade.HMRC.Extensions;
using Xunit;

namespace TipsTrade.HMRC.Tests {
  public class DateTimeExtensionTests {
    [Fact]
    public void GetTaxYear() {
      Assert.Equal("2026-27", new DateTime(2026, 4, 6).GetTaxYear());
      Assert.Equal("2026-27", new DateTime(2026, 7, 6).GetTaxYear());
      Assert.Equal("2026-27", new DateTime(2026, 10, 6).GetTaxYear());
      Assert.Equal("2026-27", new DateTime(2027, 1, 6).GetTaxYear());
    }

    [Fact]
    public void GetTaxYearEnd() {
      var expected = new DateTime(2027, 4, 5);

      Assert.Equal(expected, new DateTime(2026, 4, 6).GetTaxYearEnd());
      Assert.Equal(expected, new DateTime(2026, 7, 6).GetTaxYearEnd());
      Assert.Equal(expected, new DateTime(2026, 10, 6).GetTaxYearEnd());
      Assert.Equal(expected, new DateTime(2027, 1, 6).GetTaxYearEnd());
    }

    [Fact]
    public void GetTaxYearStart() {
      var expected = new DateTime(2026, 4, 6);

      Assert.Equal(expected, new DateTime(2026, 4, 6).GetTaxYearStart());
      Assert.Equal(expected, new DateTime(2026, 7, 6).GetTaxYearStart());
      Assert.Equal(expected, new DateTime(2026, 10, 6).GetTaxYearStart());
      Assert.Equal(expected, new DateTime(2027, 1, 6).GetTaxYearStart());
    }
  }
}
