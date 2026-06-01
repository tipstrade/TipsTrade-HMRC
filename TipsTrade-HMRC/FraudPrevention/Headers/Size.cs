using System;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Represents an object that contains size information.</summary>
  public class Size : IFraudPreventionValue {
    /// <summary>Gets or sets the height of the screen.</summary>
    public int Height { get; set; }

    /// <summary>Gets or sets the width of the screen.</summary>
    public int Width { get; set; }

    /// <summary>Returns a string that contains the fraud-prevention header value.</summary>
    public string GetHeaderValue() {
      return $"width={Width}&height={Height}";
    }

    /// <summary>Creates an instance of the <see cref="Size"/> class.</summary>
    public Size(int width, int height) {
      Width = width;
      Height = height;
    }

    /// <summary>
    /// Creates an instance of the <see cref="Size"/> class.
    /// Deprecated: use the implicit cast from <see cref="System.Drawing.Size"/>.
    /// </summary>
    [Obsolete]
    public Size(System.Drawing.Size size) : this(size.Width, size.Height) {
    }

    /// <summary>Implicitly casts a <see cref="System.Drawing.Size"/> object to a <see cref="Size"/>.</summary>
    public static implicit operator Size(System.Drawing.Size size) {
      return new Size(size.Width, size.Height);
    }
  }
}
