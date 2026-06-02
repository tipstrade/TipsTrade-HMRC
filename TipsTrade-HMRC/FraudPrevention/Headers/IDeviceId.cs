using System;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Device-ID header.</summary>
  public interface IDeviceId {
    /// <summary>Gets or sets the identifier unique to an originating device.</summary>
    Guid? DeviceId { get; set; }
  }

  internal static class DeviceIdExtensions {
    internal static (string Name, string Value) GetDeviceId(this IDeviceId source) {
      return ("Gov-Client-Device-ID", $"{source.DeviceId}");
    }
  }
}
