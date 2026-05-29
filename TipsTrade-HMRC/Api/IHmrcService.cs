namespace TipsTrade.HMRC.Api {
  /// <summary>Describes an HMRC API service that supports dependency injection.</summary>
  public interface IHmrcService : IApi {
    /// <summary>Gets the options used to configure this service.</summary>
    HmrcOptions Options { get; }
  }
}
