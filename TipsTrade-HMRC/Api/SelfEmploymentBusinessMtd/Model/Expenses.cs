using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model {
  /// <summary>Represents details about self-employment disallowable expenses, that cannot be claimed for tax purposes.</summary>
  public class ExpensesDisallowable {
    /// <summary>Payments to subcontractors - Construction Industry Scheme (CIS). Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("paymentsToSubcontractorsDisallowable"), JsonPropertyName("paymentsToSubcontractorsDisallowable")]
    public decimal PaymentsToSubcontractorsDisallowable { get; set; }

    /// <summary>Wages, salaries and other staff costs. Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("wagesAndStaffCostsDisallowable"), JsonPropertyName("wagesAndStaffCostsDisallowable")]
    public decimal WagesAndStaffCostsDisallowable { get; set; }

    /// <summary>Car, van and travel expenses. Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("carVanTravelExpensesDisallowable"), JsonPropertyName("carVanTravelExpensesDisallowable")]
    public decimal CarVanTravelExpensesDisallowable { get; set; }

    /// <summary>Rent, rates, power and insurance costs. Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("premisesRunningCostsDisallowable"), JsonPropertyName("premisesRunningCostsDisallowable")]
    public decimal PremisesRunningCostsDisallowable { get; set; }

    /// <summary>Repairs and renewals of property and equipment. Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("maintenanceCostsDisallowable"), JsonPropertyName("maintenanceCostsDisallowable")]
    public decimal MaintenanceCostsDisallowable { get; set; }

    /// <summary>Phone, fax, stationery and other office costs. Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("adminCostsDisallowable"), JsonPropertyName("adminCostsDisallowable")]
    public decimal AdminCostsDisallowable { get; set; }

    /// <summary>Business entertainment costs. Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("businessEntertainmentCostsDisallowable"), JsonPropertyName("businessEntertainmentCostsDisallowable")]
    public decimal BusinessEntertainmentCostsDisallowable { get; set; }

    /// <summary>Advertising costs. Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("advertisingCostsDisallowable"), JsonPropertyName("advertisingCostsDisallowable")]
    public decimal AdvertisingCostsDisallowable { get; set; }

    /// <summary>Interest on bank and other loans. Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("interestOnBankOtherLoansDisallowable"), JsonPropertyName("interestOnBankOtherLoansDisallowable")]
    public decimal InterestOnBankOtherLoansDisallowable { get; set; }

    /// <summary>Bank, credit card and other financial charges. Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("financeChargesDisallowable"), JsonPropertyName("financeChargesDisallowable")]
    public decimal FinanceChargesDisallowable { get; set; }

    /// <summary>Irrecoverable debts written off. Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("irrecoverableDebtsDisallowable"), JsonPropertyName("irrecoverableDebtsDisallowable")]
    public decimal IrrecoverableDebtsDisallowable { get; set; }

    /// <summary>Legal and other professional fees. Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("professionalFeesDisallowable"), JsonPropertyName("professionalFeesDisallowable")]
    public decimal ProfessionalFeesDisallowable { get; set; }

    /// <summary>Depreciation and loss/profit on sales of assets. Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("depreciationDisallowable"), JsonPropertyName("depreciationDisallowable")]
    public decimal DepreciationDisallowable { get; set; }

    /// <summary>Other business expenses. Any expense or partial expense that cannot be claimed for tax purposes.</summary>
    [JsonProperty("otherExpensesDisallowable"), JsonPropertyName("otherExpensesDisallowable")]
    public decimal OtherExpensesDisallowable { get; set; }
  }

  /// <summary>Represents details about self-employment expenses.</summary>
  public class Expenses {
    /// <summary>The sum of all allowable expenses for the specified period. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("consolidatedExpenses"), JsonPropertyName("consolidatedExpenses")]
    public decimal ConsolidatedExpenses { get; set; }

    /// <summary>Cost of goods bought for resale or goods used. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("costOfGoods"), JsonPropertyName("costOfGoods")]
    public decimal CostOfGoods { get; set; }

    /// <summary>Payments to construction Industry subcontractors. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("paymentsToSubcontractors"), JsonPropertyName("paymentsToSubcontractors")]
    public decimal PaymentsToSubcontractors { get; set; }

    /// <summary>Wages, salaries and other staff costs. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("wagesAndStaffCosts"), JsonPropertyName("wagesAndStaffCosts")]
    public decimal WagesAndStaffCosts { get; set; }

    /// <summary>Car, van and travel expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("carVanTravelExpenses"), JsonPropertyName("carVanTravelExpenses")]
    public decimal CarVanTravelExpenses { get; set; }

    /// <summary>Rent, rates, power and insurance costs. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("premisesRunningCosts"), JsonPropertyName("premisesRunningCosts")]
    public decimal PremisesRunningCosts { get; set; }

    /// <summary>Repairs and renewals of property and equipment. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("maintenanceCosts"), JsonPropertyName("maintenanceCosts")]
    public decimal MaintenanceCosts { get; set; }

    /// <summary>Phone, fax, stationery and other office costs. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("adminCosts"), JsonPropertyName("adminCosts")]
    public decimal AdminCosts { get; set; }

    /// <summary>Business entertainment costs. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("businessEntertainmentCosts"), JsonPropertyName("businessEntertainmentCosts")]
    public decimal BusinessEntertainmentCosts { get; set; }

    /// <summary>Advertising costs. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("advertisingCosts"), JsonPropertyName("advertisingCosts")]
    public decimal AdvertisingCosts { get; set; }

    /// <summary>Interest on bank and other loans. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("interestOnBankOtherLoans"), JsonPropertyName("interestOnBankOtherLoans")]
    public decimal InterestOnBankOtherLoans { get; set; }

    /// <summary>Bank, credit card and other financial charges. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("financeCharges"), JsonPropertyName("financeCharges")]
    public decimal FinanceCharges { get; set; }

    /// <summary>Irrecoverable debts written off. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("irrecoverableDebts"), JsonPropertyName("irrecoverableDebts")]
    public decimal IrrecoverableDebts { get; set; }

    /// <summary>Accountancy, legal and other professional fees. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("professionalFees"), JsonPropertyName("professionalFees")]
    public decimal ProfessionalFees { get; set; }

    /// <summary>Depreciation and loss/profit on sales of assets. Expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("depreciation"), JsonPropertyName("depreciation")]
    public decimal Depreciation { get; set; }

    /// <summary>Other business expenses associated with the running of the business.</summary>
    /// <remarks>The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.</remarks>
    [JsonProperty("otherExpenses"), JsonPropertyName("otherExpenses")]
    public decimal OtherExpenses { get; set; }
  }
}
