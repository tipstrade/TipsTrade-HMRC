using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  internal static class EncodingExtensions {
    /// <summary>
    /// Encodes a collection of key-value pairs into a URL-encoded query string format, suitable for use in HTTP headers or query parameters.
    /// </summary>
    /// <param name="values">The collection of key-value pairs to encode.</param>
    /// <param name="encodeKeys">Indicates whether the keys should be URL-encoded.</param>
    /// <param name="encodeValue">Indicates whether the values should be URL-encoded. Defaults to true.</param>
    /// <returns>A URL-encoded query string representation of the key-value pairs, or an empty string if the input is null.</returns>
    internal static string EncodeKeyValues(this IEnumerable<KeyValuePair<string, string>>? values, bool encodeKeys, bool encodeValue = true) {
      if (values == null) {
        return "";
      }

      return string.Join("&", values.Select(kvp => {
        var key = encodeKeys ? Uri.EscapeDataString(kvp.Key ?? "") : kvp.Key ?? "";
        var value = encodeValue ? Uri.EscapeDataString(kvp.Value ?? "") : kvp.Value ?? "";

        return $"{key}={value}";
      }));
    }

    /// <summary>
    /// Encodes a list of IP addresses into a comma-separated string suitable for the Gov-Client-Local-IPs header.
    /// IPv6 addresses will be percent-encoded to ensure they are transmitted correctly in HTTP headers.
    /// </summary>
    /// <param name="addresses">The list of IP addresses to encode.</param>
    /// <returns>A comma-separated string of encoded IP addresses, or an empty string if the input is null.</returns>
    internal static string EncodeIpAddresses(this IEnumerable<IPAddress>? addresses) {
      if (addresses == null) {
        return "";
      }

      // IPv6 need to be percent encoded, and IPv4 can be left as is. We can use Uri.EscapeDataString to do this.
      var asStrings = addresses.Select(x => x.EncodeIpAddress());

      return string.Join(",", asStrings);
    }

    /// <summary>
    /// Encodes a single IP address into a string suitable for the Gov-Client-Local-IPs header.
    /// </summary>
    /// <param name="address">The IP address to encode.</param>
    /// <returns>An encoded string representation of the IP address, or an empty string if the input is null.</returns>
    internal static string EncodeIpAddress(this IPAddress? address) {
      if (address == null) {
        return "";
      }

      return Uri.EscapeDataString($"{address}");
    }

    /// <summary>
    /// Encodes a list of MAC addresses into a comma-separated string suitable for the Gov-Client-MAC-Addresses header.
    /// </summary>
    /// <param name="macAddresses">The list of MAC addresses to encode.</param>
    /// <returns>A comma-separated string of encoded MAC addresses, or an empty string if the input is null.</returns>
    internal static string EncodeMacAddresses(this IEnumerable<string>? macAddresses) {
      if (macAddresses == null) {
        return "";
      }

      return string.Join(",", macAddresses.Select(Uri.EscapeDataString));
    }

    /// <summary>
    /// Encodes a DateTime value into an ISO 8601 string with milliseconds and a 'Z' suffix for UTC, suitable for the Gov-Client-Local-IPs-Timestamp header.
    /// </summary>
    /// <param name="value">The DateTime value to encode.</param>
    /// <returns>An ISO 8601 string representation of the DateTime value in UTC.</returns>
    internal static string EncodeTimestamp(this DateTime value) {
      // HMRC expects timestamps in ISO 8601 format with milliseconds and a 'Z' suffix for UTC. We can use the "o" format specifier to achieve this.
      return $"{value.ToUniversalTime():o}";
    }

    /// <summary>
    /// Encodes a TimeZoneInfo value into a string in the format "UTC±HH:MM", suitable for the Gov-Client-Timezone header.
    /// </summary>
    /// <param name="value">The TimeZoneInfo value to encode.</param>
    /// <returns>A string representation of the TimeZoneInfo value in the format "UTC±HH:MM", or an empty string if the input is null.</returns>
    internal static string EncodeTimezone(this TimeZoneInfo? value) {
      if (value == null) {
        return "";
      }

      // HMRC expects timezones in the format "UTC±HH:MM". Use GetUtcOffset to get the offset from UTC and format it accordingly.
      var offset = value.GetUtcOffset(DateTime.UtcNow);
      var sign = offset < TimeSpan.Zero ? "-" : "+";

      return $"UTC{sign}{Math.Abs(offset.Hours):D2}:{Math.Abs(offset.Minutes):D2}";
    }
  }
}
