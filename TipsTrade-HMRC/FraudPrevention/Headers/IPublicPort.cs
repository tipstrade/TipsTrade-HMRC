namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Public-Port header.</summary>
  public interface IPublicPort {
    /// <summary>Gets or sets the public TCP port that the originating device uses when initiating the request.</summary>
    int? PublicPort { get; set; }
  }

  internal static class PublicPortExtensions {
    internal static (string Name, string Value) GetPublicPort(this IPublicPort source) {
      return ("Gov-Client-Public-Port", $"{source.PublicPort}");
    }
  }
}
