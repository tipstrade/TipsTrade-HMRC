using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model {
  /// <summary>Represents the cumulative period income and expenses for a self-employment business that occurred between two dates.</summary>
  public class CumulativePeriodSummaryResult {
    /// <summary>The details about self-employment period dates.</summary>
    [JsonProperty("periodDates"), JsonPropertyName("periodDates")]
    public PeriodDates PeriodDates { get; set; }

    /// <summary>The details about self-employment income.</summary>
    [JsonProperty("periodIncome"), JsonPropertyName("periodIncome")]
    public Income PeriodIncome { get; set; }

    /// <summary>The details about self-employment expenses.</summary>
    /// <remarks>
    /// The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.
    /// For a list of all the properties, see https://developer.service.hmrc.gov.uk/api-documentation/docs/api/service/self-employment-business-api/5.0/oas/page
    /// </remarks>
    [JsonProperty("periodExpenses"), JsonPropertyName("periodExpenses")]
    public Dictionary<string, decimal> PeriodExpenses { get; set; } = null;

    /// <summary>The details about self-employment disallowable expenses, that cannot be claimed for tax purposes.</summary>
    /// <remarks>
    /// The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.
    /// For a list of all the properties, see https://developer.service.hmrc.gov.uk/api-documentation/docs/api/service/self-employment-business-api/5.0/oas/page
    /// </remarks>
    [JsonProperty("periodDisallowableExpenses"), JsonPropertyName("periodDisallowableExpenses")]
    public Dictionary<string, decimal> PeriodDisallowableExpenses { get; set; } = null;
  }

  /// <summary>Extension methods for <see cref="CumulativePeriodSummaryResult"/>.</summary>
  public static class CumulativePeriodSummaryResultExtensions {
    /// <summary>
    /// Replaces the values in the dictionary with the provided values or throws an exception if invalid keys are found.
    /// </summary>
    /// <typeparam name="T">The type of the dictionary keys.</typeparam>
    /// <typeparam name="V">The type of the dictionary values.</typeparam>
    /// <param name="dictionary">The original dictionary to update.</param>
    /// <param name="values">The values to replace in the dictionary.</param>
    /// <returns>The updated dictionary.</returns>
    /// <exception cref="ArgumentException">Thrown when invalid keys are provided in the values dictionary.</exception>
    private static Dictionary<T, V> ReplaceValueOrThrow<T, V>(this Dictionary<T, V> dictionary, Dictionary<T, V> values) {
      if (values == null) {
        return dictionary;
      }

      var invalidKeys = values.Keys.Except(dictionary.Keys);

      if (invalidKeys.Any()) {
        throw new ArgumentException($"The following keys are not valid: {string.Join(", ", invalidKeys)}. Valid keys are: {string.Join(", ", dictionary.Keys)}");
      }

      foreach (var item in values) {
        dictionary[item.Key] = item.Value;
      }

      return dictionary;
    }

    /// <summary>
    /// Adds consolidated expenses to the <see cref="CumulativePeriodSummaryResult"/> and clears disallowable expenses.
    /// </summary>
    /// <param name="result">The cumulative period summary result to update.</param>
    /// <param name="consolidatedExpenses">The consolidated expenses value to add.</param>
    /// <returns>The updated <see cref="CumulativePeriodSummaryResult"/>.</returns>
    public static CumulativePeriodSummaryResult AddConsolidatedExpenses(this CumulativePeriodSummaryResult result, decimal consolidatedExpenses) {
      result.PeriodExpenses = new Dictionary<string, decimal> {
        { "consolidatedExpenses", consolidatedExpenses }
      };
      result.PeriodDisallowableExpenses = null;

      return result;
    }

    /// <summary>
    /// Adds detailed expenses and disallowable expenses to the <see cref="CumulativePeriodSummaryResult"/>.
    /// </summary>
    /// <param name="result">The cumulative period summary result to update.</param>
    /// <param name="expenses">The detailed expenses to add. Defaults to null.</param>
    /// <param name="disallowableExpenses">The disallowable expenses to add. Defaults to null.</param>
    /// <returns>The updated <see cref="CumulativePeriodSummaryResult"/>.</returns>
    public static CumulativePeriodSummaryResult AddDetailedExpenses(this CumulativePeriodSummaryResult result, Dictionary<string, decimal> expenses = null, Dictionary<string, decimal> disallowableExpenses = null) {
      var expensesToAdd = Constants.ExpensesNames
        .Except(new string[] { "consolidatedExpenses" }) // Consolidated expenses cannot be added with detailed expenses
        .ToDictionary(x => x, x => 0M).ReplaceValueOrThrow(expenses)
        ;
      var disallowableExpensesToAdd = Constants.ExpensesDisallowableNames.ToDictionary(x => x, x => 0M).ReplaceValueOrThrow(disallowableExpenses);

      result.PeriodExpenses = expensesToAdd;
      result.PeriodDisallowableExpenses = disallowableExpensesToAdd;

      return result;
    }
  }
}
