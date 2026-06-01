using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TipsTrade.HMRC.Attributes;

namespace TipsTrade.HMRC {
  /// <summary>Provides all the scopes supported.</summary>
  public class Scopes {
    /// <summary>Provides access to the Hello World functions.</summary>
    [ScopeApi(typeof(Api.HelloWorld.HelloWorldService))]
    public const string Hello = "hello";

    /// <summary>Provides read-access to the Self-Assessment functions.</summary>
    [ScopeApi(typeof(Api.BusinessDetailsMtd.BusinessDetailsMtdService))]
    [ScopeApi(typeof(Api.ObligationsMtd.ObligationsMtdService))]
    [ScopeApi(typeof(Api.SelfAssessmentTestSupportMtd.SelfAssessmentTestSupportMtdService))]
    [ScopeApi(typeof(Api.SelfEmploymentBusinessMtd.SelfEmploymentBusinessMtdService))]
    public const string SelfAssessmentRead = "read:self-assessment";

    /// <summary>Provides write-access to the Self-Assessment functions.</summary>
    [ScopeApi(typeof(Api.BusinessDetailsMtd.BusinessDetailsMtdService))]
    [ScopeApi(typeof(Api.ObligationsMtd.ObligationsMtdService))]
    [ScopeApi(typeof(Api.SelfAssessmentTestSupportMtd.SelfAssessmentTestSupportMtdService))]
    [ScopeApi(typeof(Api.SelfEmploymentBusinessMtd.SelfEmploymentBusinessMtdService))]
    public const string SelfAssessmentWrite = "write:self-assessment";

    /// <summary>Provides read-access to the VAT functions.</summary>
    [ScopeApi(typeof(Api.Vat.VatService))]
    public const string VATRead = "read:vat";

    /// <summary>Provides write-access to the VAT functions.</summary>
    [ScopeApi(typeof(Api.Vat.VatService))]
    public const string VATWrite = "write:vat";

    /// <summary>
    /// Gets all the scopes that are valid for the specified API type, scope value, and/or scope name.
    /// </summary>
    /// <typeparam name="T">The type of API that the scope should apply to.</typeparam>
    /// <param name="valueFilter">Filters the scopes by value.</param>
    /// <param name="nameFilter">Filters the scopes by the property name in this class.</param>
    /// <returns>An enumerable of scope strings that match all of the specified filters.</returns>
    public static IEnumerable<string> GetScopes<T>(Func<string, bool>? valueFilter = null, Func<string, bool>? nameFilter = null) {
      return GetScopes(t => t == typeof(T), valueFilter, nameFilter);
    }

    /// <summary>
    /// Gets all the scopes that are valid for the specified API type, scope value, and/or scope name.
    /// </summary>
    /// <param name="typeFilter">Filters the scopes by API type.</param>
    /// <param name="valueFilter">Filters the scopes by value.</param>
    /// <param name="nameFilter">Filters the scopes by the property name in this class.</param>
    /// <returns>An enumerable of scope strings that match all of the specified filters.</returns>
    public static IEnumerable<string> GetScopes(Func<Type, bool>? typeFilter = null, Func<string, bool>? valueFilter = null, Func<string, bool>? nameFilter = null) {
      var seen = new HashSet<string>();

      var scopesFields = typeof(Scopes).GetFields(BindingFlags.Public | BindingFlags.Static);

      foreach (var field in scopesFields) {
        // Only consider string fields that haven't been seen before (to avoid duplicates).
        if (field.GetValue(null) is string strValue && !seen.Contains(strValue)) {
          var isMatch = (valueFilter?.Invoke(strValue) ?? true) // Matches the scope value filter (if provided).
            && (nameFilter?.Invoke(field.Name) ?? true) // Matches the scope name filter (if provided).
            ;

          // Exit early if the scope doesn't match the filters, to avoid unnecessary processing of attributes.
          if (!isMatch) {
            continue;
          }

          var attrs = field.GetCustomAttributes<ScopeApiAttribute>().ToArray();

          // A valid scope must have at least one ScopeApiAttribute
          if (attrs.Length == 0) {
            continue;
          }

          isMatch = typeFilter == null || attrs.Any(a => typeFilter.Invoke(a.Type)); // Matches the API type filter (if provided).

          if (isMatch && seen.Add(strValue)) {
            yield return strValue;
          }
        }
      }
    }
  }
}
