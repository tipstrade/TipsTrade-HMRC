using System;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Timezone header.</summary>
  public interface ITimeZone {
    /// <summary>Gets or sets the local timezone of the originating device.</summary>
    TimeZoneInfo? TimeZone { get; set; }
  }

  internal static class TimeZoneExtensions {
    internal static FraudPreventionHeader GetTimeZoneHeader(this ITimeZone source) =>
      new FraudPreventionHeader("Gov-Client-Timezone", false, source.TimeZone);
  }
}
