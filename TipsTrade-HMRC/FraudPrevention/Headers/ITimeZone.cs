using System;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Timezone header.</summary>
  public interface ITimeZone {
    /// <summary>Gets or sets the local timezone of the originating device.</summary>
    TimeZoneInfo? TimeZone { get; set; }
  }

  internal static class TimeZoneExtensions {
    internal static (string Name, string Value) GetTimeZone(this ITimeZone source) {
      return ("Gov-Client-Timezone", source.TimeZone.EncodeTimezone());
    }
  }
}
