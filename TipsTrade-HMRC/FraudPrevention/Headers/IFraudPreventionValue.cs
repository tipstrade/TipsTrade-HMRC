namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Specifies that the object provides a header value.</summary>
  public interface IFraudPreventionValue {
    /// <summary>
    /// Gets the value to be sent in the header. The value should be formatted as a string, and any necessary serialization should be handled by the implementation of this method.
    /// </summary>
    string GetHeaderValue();
  }
}
