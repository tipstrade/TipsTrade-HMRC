namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Browser-JS-User-Agent header.</summary>
  public interface IBrowserJavaScriptUserAgent {
    /// <summary>Gets or sets the JavaScript-reported user agent string from the originating device.</summary>
    string? BrowserJavaScriptUserAgent { get; set; }
  }

  internal static class BrowserJavaScriptUserAgentExtensions {
    internal static FraudPreventionHeader GetBrowserJavaScriptUserAgentHeader(this IBrowserJavaScriptUserAgent source) =>
      new FraudPreventionHeader("Gov-Client-Browser-JS-User-Agent", false, source.BrowserJavaScriptUserAgent);
  }
}
