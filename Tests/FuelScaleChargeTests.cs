using System;
using TipsTrade.HMRC.Api.Vat;
using TipsTrade.HMRC.Api.Vat.Model;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class FuelScaleChargeTests : TestBase {
    public FuelScaleChargeTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void TestFuelScaleChargesLive() {
      Assert.Throws<ArgumentException>(() => VatApi.GetFuelScaleChargeFromCO2Live(DateTime.Today, 2, 0));
      Assert.Throws<InvalidOperationException>(() => VatApi.GetFuelScaleChargeFromCO2Live(DateTime.MinValue, VatPeriod.Month, 0));

      FuelScaleChargeResult resp;

      // Minimum CO2 annually
      resp = VatApi.GetFuelScaleChargeFromCO2Live(new DateTime(2025, 1, 1), VatPeriod.Annual, 0);
      Assert.Equal(117m, resp.Vat);
      Assert.Equal(585m, resp.Nett);
      Assert.Equal(702, resp.ScaleCharge);
      Assert.Equal(new DateTime(2024, 5, 1), resp.From);
      Assert.Equal(new DateTime(2025, 4, 30), resp.To);
      Assert.Equal(0.2m, resp.VatRate);
      Assert.Equal(124, resp.CO2Band);

      // 167g/km CO2 quarterly
      resp = VatApi.GetFuelScaleChargeFromCO2Live(new DateTime(2025, 1, 1), VatPeriod.Quarter, 167);
      Assert.Equal(67.17m, resp.Vat);
      Assert.Equal(335.83m, resp.Nett);
      Assert.Equal(403, resp.ScaleCharge);
      Assert.Equal(new DateTime(2024, 5, 1), resp.From);
      Assert.Equal(new DateTime(2025, 4, 30), resp.To);
      Assert.Equal(0.2m, resp.VatRate);
      Assert.Equal(169, resp.CO2Band);

      // Maximum CO2 monthly
      resp = VatApi.GetFuelScaleChargeFromCO2Live(new DateTime(2025, 1, 1), VatPeriod.Month, 400);
      Assert.Equal(33.83m, resp.Vat);
      Assert.Equal(169.17m, resp.Nett);
      Assert.Equal(203, resp.ScaleCharge);
      Assert.Equal(new DateTime(2024, 5, 1), resp.From);
      Assert.Equal(new DateTime(2025, 4, 30), resp.To);
      Assert.Equal(0.2m, resp.VatRate);
      Assert.Equal(int.MaxValue, resp.CO2Band);

      // Maxium CO2 annually
      resp = VatApi.GetFuelScaleChargeFromCO2Live(new DateTime(2025, 1, 1), VatPeriod.Annual, 400);
      Assert.Equal(409m, resp.Vat);
      Assert.Equal(2045.0m, resp.Nett);
      Assert.Equal(2454m, resp.ScaleCharge);
      Assert.Equal(new DateTime(2024, 5, 1), resp.From);
      Assert.Equal(new DateTime(2025, 4, 30), resp.To);
      Assert.Equal(0.2m, resp.VatRate);
      Assert.Equal(int.MaxValue, resp.CO2Band);
    }
  }
}
