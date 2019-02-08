using System;
using System.Collections.Generic;
using TipsTrade.HMRC.AntiFraud;
using Xunit;

namespace TipsTrade.HMRC.Tests {
  public class AntiFraudTests {
    [Fact]
    public void TestGetAntiFraudHeaders() {
      var af = new TipsTrade.HMRC.AntiFraud.AntiFraud() {
        ConnectionMethod = ConnectionMethod.DESKTOP_APP_DIRECT,
        DeviceID = $"{Guid.NewGuid()}",
        Screens = new Screen[] {
          new Screen() {ColourDepth = 32, ScalingFactor=1, Size = new Size(1920,1080) }
        },
        TimeZone = TimeZoneInfo.Local,
        VendorVersion = new Dictionary<string, string>() { { "TipsTrade.HMRC.Tests", "0.0.0.1" } },
        WindowSize  = new Size(1024,768)
      };

      af.PopulateLocalIPs();
      af.PopulateMACAddresses();
      af.PopulateUserAgent();

      var headers = af.GetAntiFraudHeaders();

    }

  }
}
