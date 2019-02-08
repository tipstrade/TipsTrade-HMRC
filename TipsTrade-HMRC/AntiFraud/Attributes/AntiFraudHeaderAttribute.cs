using System;

namespace TipsTrade.HMRC.AntiFraud.Attributes {
  /// <summary>Represents an attribute that specifies the anti-fraud header information for a property.</summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class AntiFraudHeaderAttribute : Attribute {
    /// <summary>Gets or sets a flag indicating whether the header can be empty.</summary>
    public bool AllowEmpty { get; }

    /// <summary>Gets the header name of the property.</summary>
    public string HeaderName { get; }

    /// <summary>Creates an instance of the <see cref="AntiFraudHeaderAttribute"/> class.</summary>
    public AntiFraudHeaderAttribute(string headerName, bool allowEmpty = false) {
      HeaderName = headerName;
      AllowEmpty = allowEmpty;
    }
  }
}
