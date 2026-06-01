namespace TipsTrade.HMRC.FraudPrevention {
  /// <summary>Represents a single fraud prevention header name/value pair.</summary>
  public sealed class FraudPreventionHeader {
    /// <summary>Gets the HTTP header name.</summary>
    public string Name { get; }

    /// <summary>Gets a flag indicating whether the header value may be empty.</summary>
    public bool AllowEmpty { get; }

    /// <summary>Gets the raw value of the header.</summary>
    public object? Value { get; }

    /// <summary>Creates an instance of the <see cref="FraudPreventionHeader"/> class.</summary>
    public FraudPreventionHeader(string name, bool allowEmpty, object? value) {
      Name = name;
      AllowEmpty = allowEmpty;
      Value = value;
    }

    /// <inheritdoc/>
    public override string ToString() {
      return $"{Name}: {(Value is null ? "null" : Value.ToString())}";
    }
  }
}
