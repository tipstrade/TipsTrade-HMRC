using System;
using System.Collections.Generic;
using System.Linq;
using TipsTrade.HMRC.AntiFraud;
using Xunit;

namespace TipsTrade.HMRC.Tests {
  public class AntiFraudTests {
    [Fact]
    public void TestAntiFraudHeaderValidation() {
      var af = new AntiFraud.AntiFraud() {
        ConnectionMethod = ConnectionMethod.DESKTOP_APP_DIRECT,
      };

      const int expectedErrors = 4;

      var ex = Assert.Throws<AntiFraudException>(() => af.GetAntiFraudHeaders());
      Assert.Equal(expectedErrors, ex.Errors.Count());

      Assert.False(af.Validate(out string[] errors));
      Assert.Equal(expectedErrors, errors.Count());
    }

    [Fact]
    public void TestGetAntiFraudHeaders() {
      var af = new AntiFraud.AntiFraud() {
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

    [Fact]
    public void TestGetPropertiesForMethod() {
      var props = AntiFraud.AntiFraud.GetPropertiesForMethod(ConnectionMethod.DESKTOP_APP_DIRECT);
      Assert.Equal(12, props.Count());
    }
  }
}
