using System.Collections.Generic;
using System.Threading;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model {
  public static class Constants {
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
