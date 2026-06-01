namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Window-Size header.</summary>
  public interface IWindowSize {
    /// <summary>Gets or sets the number of pixels of the window on the originating device.</summary>
    Size? WindowSize { get; set; }
  }

  internal static class WindowSizeExtensions {
    internal static FraudPreventionHeader GetWindowSizeHeader(this IWindowSize source) =>
      new FraudPreventionHeader("Gov-Client-Window-Size", true, source.WindowSize);
  }
}
