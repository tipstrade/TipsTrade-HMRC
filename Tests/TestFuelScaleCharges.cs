using System;
using TipsTrade.HMRC.Api.Vat;
using TipsTrade.HMRC.Api.Vat.Model;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class TestFuelScaleCharge : TestBase {
    public TestFuelScaleCharge(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void TestFuelScaleCharges() {
      Assert.Throws<ArgumentException>(() => VatApi.GetFuelScaleChargeFromCO2(DateTime.Today, 2, 0));
      Assert.Throws<InvalidOperationException>(() => VatApi.GetFuelScaleChargeFromCO2(DateTime.MinValue, VatPeriod.Month, 0));

      FuelScaleChargeResult resp;

      // Minimum CO2 annually
      resp = VatApi.GetFuelScaleChargeFromCO2(new DateTime(2018, 12, 31), VatPeriod.Annual, 0);
      Assert.Equal(93.67m, resp.Vat);
      Assert.Equal(468.33m, resp.Nett);
      Assert.Equal(562, resp.ScaleCharge);
      Assert.Equal(new DateTime(2018, 5, 1), resp.From);
      Assert.Equal(new DateTime(2019, 4, 30), resp.To);
      Assert.Equal(0.2m, resp.VatRate);
      Assert.Equal(124, resp.CO2Band);

      // 167g/km CO2 quarterly
      resp = VatApi.GetFuelScaleChargeFromCO2(new DateTime(2018, 12, 31), VatPeriod.Quarter, 167);
      Assert.Equal(53.83m, resp.Vat);
      Assert.Equal(269.17m, resp.Nett);
      Assert.Equal(323, resp.ScaleCharge);
      Assert.Equal(new DateTime(2018, 5, 1), resp.From);
      Assert.Equal(new DateTime(2019, 4, 30), resp.To);
      Assert.Equal(0.2m, resp.VatRate);
      Assert.Equal(169, resp.CO2Band);

      // Maximum CO2 monthly
      resp = VatApi.GetFuelScaleChargeFromCO2(new DateTime(2018, 12, 31), VatPeriod.Month, 400);
      Assert.Equal(27.17m, resp.Vat);
      Assert.Equal(135.83m, resp.Nett);
      Assert.Equal(163, resp.ScaleCharge);
      Assert.Equal(new DateTime(2018, 5, 1), resp.From);
      Assert.Equal(new DateTime(2019, 4, 30), resp.To);
      Assert.Equal(0.2m, resp.VatRate);
      Assert.Equal(int.MaxValue, resp.CO2Band);
    }

    [Fact]
    public void TestFuelScaleChargesLive() {
      Assert.Throws<ArgumentException>(() => VatApi.GetFuelScaleChargeFromCO2Live(DateTime.Today, 2, 0));
      Assert.Throws<InvalidOperationException>(() => VatApi.GetFuelScaleChargeFromCO2Live(DateTime.MinValue, VatPeriod.Month, 0));

      FuelScaleChargeResult resp;

      // Minimum CO2 annually
      resp = VatApi.GetFuelScaleChargeFromCO2Live(new DateTime(2018, 12, 31), VatPeriod.Annual, 0);
      Assert.Equal(93.67m, resp.Vat);
      Assert.Equal(468.33m, resp.Nett);
      Assert.Equal(562, resp.ScaleCharge);
      Assert.Equal(new DateTime(2018, 5, 1), resp.From);
      Assert.Equal(new DateTime(2019, 4, 30), resp.To);
      Assert.Equal(0.2m, resp.VatRate);
      Assert.Equal(124, resp.CO2Band);

      // 167g/km CO2 quarterly
      resp = VatApi.GetFuelScaleChargeFromCO2Live(new DateTime(2018, 12, 31), VatPeriod.Quarter, 167);
      Assert.Equal(53.83m, resp.Vat);
      Assert.Equal(269.17m, resp.Nett);
      Assert.Equal(323, resp.ScaleCharge);
      Assert.Equal(new DateTime(2018, 5, 1), resp.From);
      Assert.Equal(new DateTime(2019, 4, 30), resp.To);
      Assert.Equal(0.2m, resp.VatRate);
      Assert.Equal(169, resp.CO2Band);

      // Maximum CO2 monthly
      resp = VatApi.GetFuelScaleChargeFromCO2Live(new DateTime(2018, 12, 31), VatPeriod.Month, 400);
      Assert.Equal(27.17m, resp.Vat);
      Assert.Equal(135.83m, resp.Nett);
      Assert.Equal(163, resp.ScaleCharge);
      Assert.Equal(new DateTime(2018, 5, 1), resp.From);
      Assert.Equal(new DateTime(2019, 4, 30), resp.To);
      Assert.Equal(0.2m, resp.VatRate);
      Assert.Equal(int.MaxValue, resp.CO2Band);
    }
  }
}
