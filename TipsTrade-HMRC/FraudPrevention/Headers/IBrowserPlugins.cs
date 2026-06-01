using System.Collections.Generic;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Browser-Plugins header.</summary>
  public interface IBrowserPlugins {
    /// <summary>Gets or sets the list of browser plugins on the originating device.</summary>
    IEnumerable<string>? BrowserPlugins { get; set; }
  }

  internal static class BrowserPluginsExtensions {
    internal static FraudPreventionHeader GetBrowserPluginsHeader(this IBrowserPlugins source) =>
      new FraudPreventionHeader("Gov-Client-Browser-Plugins", true, source.BrowserPlugins);
  }
}
