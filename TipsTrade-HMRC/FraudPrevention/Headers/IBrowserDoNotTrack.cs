namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Browser-Do-Not-Track header.</summary>
  public interface IBrowserDoNotTrack {
    /// <summary>Gets or sets a flag indicating whether the Do Not Track option is enabled on the browser.</summary>
    bool? BrowserDoNotTrack { get; set; }
  }

  internal static class BrowserDoNotTrackExtensions {
    internal static FraudPreventionHeader GetBrowserDoNotTrackHeader(this IBrowserDoNotTrack source) =>
      new FraudPreventionHeader("Gov-Client-Browser-Do-Not-Track", true, source.BrowserDoNotTrack);
  }
}
