namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Window-Size header.</summary>
  public interface IWindowSize {
    /// <summary>Gets or sets the number of pixels of the window on the originating device.</summary>
    Size? WindowSize { get; set; }
  }

  internal static class WindowSizeExtensions {
    internal static (string Name, string Value) GetWindowSize(this IWindowSize source) {
      var value = source.WindowSize == null ? "" : source.WindowSize.GetHeaderValue();

      return ("Gov-Client-Window-Size", value);
    }
  }
}
