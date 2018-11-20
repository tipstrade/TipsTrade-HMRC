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
        .Where(f=> f.FieldType == typeof(string))
        ;

      if ((typeFilter != null) || (nameFilter != null)) {
        fields = fields.Where(f => {
          return (typeFilter?.Invoke(f.GetCustomAttribute<ScopeApiAttribute>()?.Type) ?? true) && (nameFilter?.Invoke(f.Name) ?? true);
        });
      }

      var scopes = fields.Select(f => (string)f.GetValue(null));

      if (valueFilter != null) {
        scopes = scopes.Where(valueFilter);
      }

      return scopes;
    }
  }
}
