using System;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>
  /// Represents a result containing Fuel Scale Charge information.
  /// See <see cref="!:https://www.gov.uk/government/publications/vat-road-fuel-scale-charges-table"></see>
  /// </summary>
  public class FuelScaleChargeResult {
    /// <summary>The CO2 band.</summary>
    public int CO2Band { get; set; }

    /// <summary>The date from when the Scale Charge is valid.</summary>
    public DateTime From { get; set; }

    /// <summary>The VAT Fuel Scale Charge.</summary>
    public decimal ScaleCharge => Nett + Vat;

    /// <summary>The date up to when the Scale Charge is valid.</summary>
    public DateTime To { get; set; }

    /// <summary>The amount of VAT on the Fuel Scale Charge.</summary>
    public decimal Vat { get; set; }

    /// <summary>The amount of VAT-exclusive on the Fuel Scale Charge.</summary>
    public decimal Nett { get; set; }

    /// <summary>
    /// The VAT rate for the Fuel Scale Charge.
    /// Note: This is an estimate rounded to 0.1%
    /// </summary>
    public decimal VatRate => Math.Round(Vat / Nett, 3);

    /// <summary>Creates an instance of the FuelScaleCharge class.</summary>
    public FuelScaleChargeResult() {
    }
  }
}
