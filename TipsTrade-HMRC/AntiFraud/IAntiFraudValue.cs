namespace TipsTrade.HMRC.AntiFraud {
  /// <summary>Specifies that the object provides a header value.</summary>
  public interface IAntiFraudValue {
    /// <summary>Retuns a string that contains the anti fraud header value.</summary>
    string GetHeaderValue();
  }
}
