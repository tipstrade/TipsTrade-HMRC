using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
using TipsTrade.HMRC.FraudPrevention.Headers;

namespace TipsTrade.HMRC.Tests {
  public class EncodingExtensionsTests {

    // EncodeKeyValues

    [Test]
    public void EncodeKeyValues_NullInput_ReturnsEmptyString() {
      IDictionary<string, string>? dict = null;
      Assert.That(dict.EncodeKeyValues(encodeKeys: false), Is.EqualTo(""));
    }

    [Test]
    public void EncodeKeyValues_SimpleValues_NoEncoding() {
      var dict = new Dictionary<string, string> {
        { "key1", "value1" },
        { "key2", "value2" }
      }.OrderBy(x => x.Key);
      Assert.That(dict.EncodeKeyValues(encodeKeys: false, encodeValue: false), Is.EqualTo("key1=value1&key2=value2"));
    }

    [Test]
    public void EncodeKeyValues_EncodesValues_WhenRequested() {
      var dict = new Dictionary<string, string> {
        { "key", "hello world" }
      };
      Assert.That(dict.EncodeKeyValues(encodeKeys: false), Is.EqualTo("key=hello%20world"));
    }

    [Test]
    public void EncodeKeyValues_EncodesKeys_WhenRequested() {
      var dict = new Dictionary<string, string> {
        { "my key", "value" }
      };
      Assert.That(dict.EncodeKeyValues(encodeKeys: true, encodeValue: false), Is.EqualTo("my%20key=value"));
    }

    // EncodeIpAddresses

    [Test]
    public void EncodeIpAddresses_NullInput_ReturnsEmptyString() {
      IEnumerable<IPAddress>? addresses = null;
      Assert.That(addresses.EncodeIpAddresses(), Is.EqualTo(""));
    }

    [Test]
    public void EncodeIpAddresses_IPv4_ReturnsCommaSeparated() {
      var addresses = new[] {
        IPAddress.Parse("192.168.0.1"),
        IPAddress.Parse("10.0.0.1")
      };
      Assert.That(addresses.EncodeIpAddresses(), Is.EqualTo("192.168.0.1,10.0.0.1"));
    }

    [Test]
    public void EncodeIpAddresses_IPv6_IsPercentEncoded() {
      var addresses = new[] { IPAddress.Parse("::1") };
      var result = addresses.EncodeIpAddresses();
      Assert.That(result, Does.Not.Contain(":"), "IPv6 colons should be percent-encoded");
      Assert.That(result, Does.Contain("%"));
    }

    // EncodeIpAddress

    [Test]
    public void EncodeIpAddress_NullInput_ReturnsEmptyString() {
      IPAddress? address = null;
      Assert.That(address.EncodeIpAddress(), Is.EqualTo(""));
    }

    [Test]
    public void EncodeIpAddress_IPv4_ReturnsUnchanged() {
      Assert.That(IPAddress.Parse("192.168.1.1").EncodeIpAddress(), Is.EqualTo("192.168.1.1"));
    }

    // EncodeMacAddresses

    [Test]
    public void EncodeMacAddresses_NullInput_ReturnsEmptyString() {
      IEnumerable<string>? macs = null;
      Assert.That(macs.EncodeMacAddresses(), Is.EqualTo(""));
    }

    [Test]
    public void EncodeMacAddresses_SingleAddress_ReturnsEncoded() {
      var macs = new[] { "00:1A:2B:3C:4D:5E" };
      Assert.That(macs.EncodeMacAddresses(), Is.EqualTo("00%3A1A%3A2B%3A3C%3A4D%3A5E"));
    }

    [Test]
    public void EncodeMacAddresses_MultipleAddresses_ReturnsCommaSeparated() {
      var macs = new[] { "00:1A:2B:3C:4D:5E", "FF:EE:DD:CC:BB:AA" };
      var result = macs.EncodeMacAddresses();
      Assert.That(result, Does.Contain(","));
      Assert.That(result.Split(','), Has.Length.EqualTo(2));
    }

    // EncodeTimestamp

    [Test]
    public void EncodeTimestamp_ReturnsIso8601WithZSuffix() {
      var dt = new DateTime(2026, 6, 2, 12, 30, 45, 123, DateTimeKind.Utc);
      var result = dt.EncodeTimestamp();
      Assert.That(result, Does.EndWith("Z"));
      Assert.That(result, Does.Contain("2026-06-02"));
    }

    [Test]
    public void EncodeTimestamp_LocalTime_ConvertedToUtc() {
      var dt = new DateTime(2026, 6, 2, 12, 0, 0, DateTimeKind.Local);
      var result = dt.EncodeTimestamp();
      Assert.That(result, Does.EndWith("Z"));
    }

    // EncodeTimezone

    [Test]
    public void EncodeTimezone_NullInput_ReturnsEmptyString() {
      TimeZoneInfo? tz = null;
      Assert.That(tz.EncodeTimezone(), Is.EqualTo(""));
    }

    [Test]
    public void EncodeTimezone_Utc_ReturnsUtcPlus0000() {
      Assert.That(TimeZoneInfo.Utc.EncodeTimezone(), Is.EqualTo("UTC+00:00"));
    }

    [Test]
    public void EncodeTimezone_PositiveOffset_FormattedCorrectly() {
      // UTC+05:30 (India Standard Time equivalent)
      var tz = TimeZoneInfo.CreateCustomTimeZone("test", TimeSpan.FromHours(5.5), "Test", "Test");
      Assert.That(tz.EncodeTimezone(), Is.EqualTo("UTC+05:30"));
    }

    [Test]
    public void EncodeTimezone_NegativeOffset_FormattedCorrectly() {
      var tz = TimeZoneInfo.CreateCustomTimeZone("test-neg", TimeSpan.FromHours(-5), "Test Neg", "Test Neg");
      Assert.That(tz.EncodeTimezone(), Is.EqualTo("UTC-05:00"));
    }
  }
}