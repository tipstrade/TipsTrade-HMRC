namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Device-ID header.</summary>
  public interface IDeviceId {
    /// <summary>Gets or sets the identifier unique to an originating device.</summary>
    string? DeviceId { get; set; }
  }

  internal static class DeviceIdExtensions {
    internal static FraudPreventionHeader GetDeviceIdHeader(this IDeviceId source) =>
      new FraudPreventionHeader("Gov-Client-Device-ID", true, source.DeviceId);
  }
}
