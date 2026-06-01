namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Represents an object that contains client screen information.</summary>
  public class Screen : IFraudPreventionValue {
    /// <summary>Gets or sets the colour depth of the screen.</summary>
    public int ColourDepth { get; set; }

    /// <summary>Gets or sets the reported scaling factor of the screen.</summary>
    public float ScalingFactor { get; set; }

    /// <summary>Gets or sets the dimensions of the screen.</summary>
    public Size Size { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Screen"/> class with the specified width and height.
    /// </summary>
    public Screen(int width, int height, int colourDepth, float scalingFactor) : this(new Size(width, height), colourDepth, scalingFactor) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Screen"/> class with the specified size.
    /// </summary>
    public Screen(Size size, int colourDepth, float scalingFactor) {
      Size = size;
      ColourDepth = colourDepth;
      ScalingFactor = scalingFactor;
    }

    /// <summary>Returns a string that contains the fraud prevention header value.</summary>
    public string GetHeaderValue() {
      // Ensure the scaling factor is formatted with a dot as the decimal separator, regardless of the current culture.
      var scalingText = ScalingFactor.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);

      // Ensure the scaling factor is formatted with a dot as the decimal separator, regardless of the current culture.
      return $"width={Size.Width}&height={Size.Height}&scaling-factor={scalingText}&colour-depth={ColourDepth}";
    }

#if NETFRAMEWORK
    /// <summary>Implicitly casts a <see cref="System.Windows.Forms.Screen"/> object to a <see cref="Screen"/>.</summary>
    public static implicit operator Screen(System.Windows.Forms.Screen screen) {
      return new Screen(screen.Bounds.Width, screen.Bounds.Height, screen.BitsPerPixel, 1);
    }
#endif
  }
}
