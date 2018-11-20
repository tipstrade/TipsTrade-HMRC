using System;

namespace TipsTrade.HMRC.Attributes {
  /// <summary>Represents an attribute that indicates that the API type that the scope applies to.</summary>
  [AttributeUsage(AttributeTargets.Field)]
  public class ScopeApiAttribute : Attribute {
    /// <summary>The type that the scope applies to.</summary>
    public Type Type { get; }

    /// <summary>Creates an instance of the AppliesToAttribute class.</summary>
    public ScopeApiAttribute(Type type) {
      Type = type;
    }
  }
}
