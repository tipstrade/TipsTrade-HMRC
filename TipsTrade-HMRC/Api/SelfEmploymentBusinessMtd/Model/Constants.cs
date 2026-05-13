using System.Collections.Generic;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model {
  /// <summary>
  /// Provides a set of constant property names used by the Self Employment Business MTD model.
  /// </summary>
  public static class Constants {
    /// <summary>
    /// The canonical names of allowable expense properties used by the Self Employment Business MTD model.
    /// </summary>
    public static readonly IReadOnlyCollection<string> ExpensesNames = new string[] {
      "consolidatedExpenses",
      "costOfGoods",
      "paymentsToSubcontractors",
      "wagesAndStaffCosts",
      "carVanTravelExpenses",
      "premisesRunningCosts",
      "maintenanceCosts",
      "adminCosts",
      "businessEntertainmentCosts",
      "advertisingCosts",
      "interestOnBankOtherLoans",
      "financeCharges",
      "irrecoverableDebts",
      "professionalFees",
      "depreciation",
      "otherExpenses"
    };

    /// <summary>
    /// The canonical names of disallowable expense properties used by the Self Employment Business MTD model.
    /// </summary>
    public static readonly IReadOnlyCollection<string> ExpensesDisallowableNames = new string[] {
      "costOfGoodsDisallowable",
      "paymentsToSubcontractorsDisallowable",
      "wagesAndStaffCostsDisallowable",
      "carVanTravelExpensesDisallowable",
      "premisesRunningCostsDisallowable",
      "maintenanceCostsDisallowable",
      "adminCostsDisallowable",
      "businessEntertainmentCostsDisallowable",
      "advertisingCostsDisallowable",
      "interestOnBankOtherLoansDisallowable",
      "financeChargesDisallowable",
      "irrecoverableDebtsDisallowable",
      "professionalFeesDisallowable",
      "depreciationDisallowable",
      "otherExpensesDisallowable"
    };
  }
}
