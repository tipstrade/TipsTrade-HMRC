namespace TipsTrade.HMRC.AntiFraud {
  /// <summary>Represents an object that contains client screen information.</summary>
  public class Screen : IAntiFraudValue {
    /// <summary>Gets or sets the colour depth of the screen.</summary>
    public int? ColourDepth { get; set; }

    /// <summary>Gets or sets the reported scaling factor of the screen.</summary>
    public float? ScalingFactor { get; set; }

    /// <summary>Gets or sets the dimensions of the screen.</summary>
    public Size Size { get; set; }

    /// <summary>Retuns a string that contains the anti fraud header value.</summary>
    public string GetHeaderValue() {
      return $"width={Size.Width}&height={Size.Height}&scaling-factor={ScalingFactor}&colour-depth={ColourDepth}";
    }

#if NET452
    /// <summary>Implicitly casts a <see cref="System.Windows.Forms.Screen"/> object to a <see cref="Screen"/>.</summary>
    public static implicit operator Screen(System.Windows.Forms.Screen screen) {
      return new Screen() {
        ColourDepth = screen.BitsPerPixel,
        ScalingFactor = 1,
        Size = screen.Bounds.Size
      };
    }
#endif
  }
}
