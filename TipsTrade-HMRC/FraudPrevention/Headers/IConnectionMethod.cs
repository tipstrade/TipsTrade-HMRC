namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Connection-Method header.</summary>
  public interface IConnectionMethod {
    /// <summary>Gets the connection method being used.</summary>
    ConnectionMethod ConnectionMethod { get; }
  }

  internal static class ConnectionMethodExtensions {
    internal static FraudPreventionHeader GetConnectionMethodHeader(this IConnectionMethod source) =>
      new FraudPreventionHeader("Gov-Client-Connection-Method", false, source.ConnectionMethod);
  }
}
