using System;
using TipsTrade.HMRC.Extensions;
using NUnit.Framework;

namespace TipsTrade.HMRC.Tests {
  public class DateTimeExtensionTests {
    [Test]
    public void GetTaxYear() {
      Assert.That(new DateTime(2026, 4, 6).GetTaxYear(), Is.EqualTo("2026-27"));
      Assert.That(new DateTime(2026, 7, 6).GetTaxYear(), Is.EqualTo("2026-27"));
      Assert.That(new DateTime(2026, 10, 6).GetTaxYear(), Is.EqualTo("2026-27"));
      Assert.That(new DateTime(2027, 1, 6).GetTaxYear(), Is.EqualTo("2026-27"));
    }

    [Test]
    public void GetTaxYearEnd() {
      var expected = new DateTime(2027, 4, 5);

      Assert.That(new DateTime(2026, 4, 6).GetTaxYearEnd(), Is.EqualTo(expected));
      Assert.That(new DateTime(2026, 7, 6).GetTaxYearEnd(), Is.EqualTo(expected));
      Assert.That(new DateTime(2026, 10, 6).GetTaxYearEnd(), Is.EqualTo(expected));
      Assert.That(new DateTime(2027, 1, 6).GetTaxYearEnd(), Is.EqualTo(expected));
    }

    [Test]
    public void GetTaxYearStart() {
      var expected = new DateTime(2026, 4, 6);

      Assert.That(new DateTime(2026, 4, 6).GetTaxYearStart(), Is.EqualTo(expected));
      Assert.That(new DateTime(2026, 7, 6).GetTaxYearStart(), Is.EqualTo(expected));
      Assert.That(new DateTime(2026, 10, 6).GetTaxYearStart(), Is.EqualTo(expected));
      Assert.That(new DateTime(2027, 1, 6).GetTaxYearStart(), Is.EqualTo(expected));
    }
  }
}
