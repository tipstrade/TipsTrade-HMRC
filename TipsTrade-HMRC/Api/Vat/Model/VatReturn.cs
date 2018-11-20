using Newtonsoft.Json;

namespace TipsTrade.HMRC.Api.Vat.Model {
  /// <summary>Represents a result containing a VAT return.</summary>
  public class VatReturn {
    /// <summary>Declaration that the user has finalised their VAT return.</summary>
    [JsonProperty("finalised")]
    public bool? Finalised { get; set; }

    /// <summary>
    /// The ID code for the period that this obligation belongs to.
    /// The format is a string of four alphanumeric characters.
    /// Occasionally the format includes the # symbol.
    /// </summary>
    [JsonProperty("periodKey")]
    public string PeriodKey { get; set; }

    /// <summary>
    /// VAT due on sales and other outputs.
    /// This corresponds to box 1 on the VAT Return form.
    /// </summary>
    [JsonProperty("vatDueSales")]
    public decimal VatDueSales { get; set; }

    /// <summary>
    /// VAT due on acquisitions from other EC Member States.
    /// This corresponds to box 2 on the VAT Return form.
    /// </summary>
    [JsonProperty("vatDueAcquisitions")]
    public decimal VatDueAcquisitions { get; set; }

    /// <summary>
    /// Total VAT due (the sum of VatDueSales and VatDueAcquisitions).
    /// This corresponds to box 3 on the VAT Return form.
    /// </summary>
    [JsonProperty("totalVatDue")]
    public decimal TotalVatDue { get; set; }

    /// <summary>
    /// VAT reclaimed on purchases and other inputs (including acquisitions from the EC).
    /// This corresponds to box 4 on the VAT Return form.
    /// </summary>
    [JsonProperty("vatReclaimedCurrPeriod")]
    public decimal VatReclaimedCurrPeriod { get; set; }

    /// <summary>
    /// The difference between totalVatDue and vatReclaimedCurrPeriod.
    /// This corresponds to box 5 on the VAT Return form.
    /// </summary>
    [JsonProperty("netVatDue")]
    public decimal NetVatDue { get; set; }

    /// <summary>
    /// Total value of sales and all other outputs excluding any VAT.
    /// This corresponds to box 6 on the VAT Return form.
    /// </summary>
    [JsonProperty("totalValueSalesExVAT")]
    public long TotalValueSalesExVAT { get; set; }

    /// <summary>
    /// Total value of purchases and all other inputs excluding any VAT (including exempt purchases).
    /// This corresponds to box 7 on the VAT Return form. 
    /// </summary>
    [JsonProperty("totalValuePurchasesExVAT")]
    public long TotalValuePurchasesExVAT { get; set; }

    /// <summary>
    /// Total value of all supplies of goods and related costs, excluding any VAT, to other EC member states.
    /// This corresponds to box 8 on the VAT Return form.
    /// </summary>
    [JsonProperty("totalValueGoodsSuppliedExVAT")]
    public long TotalValueGoodsSuppliedExVAT { get; set; }

    /// <summary>
    /// Total value of acquisitions of goods and related costs excluding any VAT, from other EC member states.
    /// This corresponds to box 9 on the VAT Return form.
    /// </summary>
    [JsonProperty("totalAcquisitionsExVAT")]
    public long TotalAcquisitionsExVAT { get; set; }

    /// <summary>Returns a string that represents the current object.</summary>
    public override string ToString() {
      return $"Period: {PeriodKey}, NetVatDue: {NetVatDue}";
    }
  }
}
