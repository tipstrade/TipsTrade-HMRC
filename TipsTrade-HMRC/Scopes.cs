using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TipsTrade.HMRC.Attributes;

namespace TipsTrade.HMRC {
  /// <summary>Provides all the scopes supported.</summary>
  public class Scopes {
    /// <summary>Provides access to the Hello World functions.</summary>
    [ScopeApi(typeof(Api.HelloWorld.HelloWorldApi))]
    public const string Hello = "hello";

    /// <summary>Provides read-access to the Self-Assessment functions.</summary>
    [ScopeApi(typeof(Api.BusinessDetailsMtd.BusinessDetailsMtdApi))]
    [ScopeApi(typeof(Api.ObligationsMtd.ObligationsMtdApi))]
    [ScopeApi(typeof(Api.SelfAssessmentTestSupportMtd.SelfAssessmentTestSupportMtdApi))]
    [ScopeApi(typeof(Api.SelfEmploymentBusinessMtd.SelfEmploymentBusinessMtdApi))]
    public const string SelfAssessmentRead = "read:self-assessment";

    /// <summary>Provides write-access to the Self-Assessment functions.</summary>
    [ScopeApi(typeof(Api.BusinessDetailsMtd.BusinessDetailsMtdApi))]
    [ScopeApi(typeof(Api.ObligationsMtd.ObligationsMtdApi))]
    [ScopeApi(typeof(Api.SelfAssessmentTestSupportMtd.SelfAssessmentTestSupportMtdApi))]
    [ScopeApi(typeof(Api.SelfEmploymentBusinessMtd.SelfEmploymentBusinessMtdApi))]
    public const string SelfAssessmentWrite = "write:self-assessment";

    /// <summary>Provides read-access to the VAT functions.</summary>
    [ScopeApi(typeof(Api.Vat.VatApi))]
    public const string VATRead = "read:vat";

    /// <summary>Provides write-access to the VAT functions.</summary>
    [ScopeApi(typeof(Api.Vat.VatApi))]
    public const string VATWrite = "write:vat";

    /// <summary>Gets all the scopes that are valid for the specified Api type.</summary>
    /// <typeparam name="T">The type of API that the scope should apply to.</typeparam>
    /// <param name="valueFilter">Filters the scopes by value.</param>
    /// <param name="nameFilter">Filters the scopes by name.</param>
    public static IEnumerable<string> GetScopes<T>(Func<string, bool> valueFilter = null, Func<string, bool> nameFilter = null) {
      return GetScopes(t => t == typeof(T), valueFilter, nameFilter);
    }

    /// <summary>Gets all the scopes.</summary>
    /// <param name="typeFilter">Filters the scopes by API type.</param>
    /// <param name="nameFilter">Filters the scopes by name.</param>
    /// <param name="valueFilter">Filters the scopes by value.</param>
    public static IEnumerable<string> GetScopes(Func<Type, bool> typeFilter = null, Func<string, bool> valueFilter = null, Func<string, bool> nameFilter = null) {
      var fields = typeof(Scopes)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(f => new {
          Field = f,
          Attributes = f.GetCustomAttributes<ScopeApiAttribute>()
        })
        .Where(f => f.Attributes.Any())
       ;

      var scopes = new HashSet<string>(); 

      foreach (var field in fields) {
        foreach (var scopeAttr in field.Attributes) {
          var value = (string)field.Field.GetValue(null);

          var isMatch = (typeFilter?.Invoke(scopeAttr.Type) ?? true)
            && (valueFilter?.Invoke(value) ?? true)
            && (nameFilter?.Invoke(field.Field.Name) ?? true)
            ;

          if (isMatch) {
            scopes.Add(value);
          }
        }
      }

      return scopes;
    }
  }
}
