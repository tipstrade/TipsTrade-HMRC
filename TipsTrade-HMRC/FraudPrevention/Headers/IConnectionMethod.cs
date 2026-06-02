namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Connection-Method header.</summary>
  public interface IConnectionMethod {
    /// <summary>Gets the connection method being used.</summary>
    ConnectionMethod ConnectionMethod { get; }
  }

  internal static class ConnectionMethodExtensions {
    internal static (string Name, string Value) GetConnectionMethod(this IConnectionMethod source) {
      return ("Gov-Client-Connection-Method", $"{source.ConnectionMethod}");
    }
  }
}
