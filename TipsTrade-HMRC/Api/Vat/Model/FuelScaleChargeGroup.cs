using System;
using System.Collections.Generic;
using System.Text;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a result containing Fuel Scale Charge information.</summary>
  public class FuelScaleChargeGroup {
    /// <summary>The list of Scale Charges for the period for returns filed annually.</summary>
    public List<FuelScaleChargeResult> Annually { get; set; } = new List<FuelScaleChargeResult>();

    /// <summary>The date from when the Scale Charges are valid.</summary>
    public DateTime From { get; set; }

    /// <summary>The list of Scale Charges for the period for returns filed monthly.</summary>
    public List<FuelScaleChargeResult> Monthly { get; set; } = new List<FuelScaleChargeResult>();

    /// <summary>The list of Scale Charges for the period for returns filed quarterly.</summary>
    public List<FuelScaleChargeResult> Quarterly { get; set; } = new List<FuelScaleChargeResult>();

    /// <summary>The date up to when the Scale Charges are valid.</summary>
    public DateTime To { get; set; }
  }
}
