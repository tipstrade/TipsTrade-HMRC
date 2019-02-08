using System;

namespace TipsTrade.HMRC.AntiFraud.Attributes {
  /// <summary>Represents an attribute that specifies the type of <see cref="ConnectionMethod"/> a property applies to.</summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
  public class ConnectionMethodAttribute : Attribute {
    /// <summary>Gets a flag indicating whether all connection methods are allowed.</summary>
    public bool AllConnectionMethods { get; }

    /// <summary>Gets the connection method if the property.</summary>
    public ConnectionMethod? ConnectionMethod { get; }

    /// <summary>Creates an instance of the <see cref="ConnectionMethodAttribute"/> class.</summary>
    /// <param name="connectionMethod">The <see cref="ConnectionMethod"/>.</param>
    public ConnectionMethodAttribute(ConnectionMethod connectionMethod) {
      ConnectionMethod = connectionMethod;
    }

    /// <summary>Creates an instance of the <see cref="ConnectionMethodAttribute"/> class.</summary>
    /// <param name="allConnectionMethods">A flag indicating whether the attribute applies to all connection methods.</param>
    public ConnectionMethodAttribute(bool allConnectionMethods = false) {
      AllConnectionMethods = allConnectionMethods;
    }

    /// <summary>Returns a flag indicating whether this attribute is required for the specified <see cref="ConnectionMethod"/>.</summary>
    public bool IsRequired(ConnectionMethod connectionMethod) {
      return AllConnectionMethods || (ConnectionMethod == connectionMethod);
    }
  }
}
