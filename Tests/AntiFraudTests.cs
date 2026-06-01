using System;
using System.Collections.Generic;
using System.Linq;
using TipsTrade.HMRC.AntiFraud;
using NUnit.Framework;

namespace TipsTrade.HMRC.Tests {
  public class AntiFraudTests {
    [Test]
    public void AntiFraudHeaderValidation() {
      var af = new AntiFraud.AntiFraud() {
        ConnectionMethod = ConnectionMethod.DESKTOP_APP_DIRECT,
      };

      const int expectedErrors = 4;

      var ex = Assert.Throws<AntiFraudException>((Action)(() => af.GetAntiFraudHeaders()));
      Assert.That(ex.Errors.Count(), Is.EqualTo(expectedErrors));

      Assert.That(af.Validate(out var errors), Is.False);
      Assert.That(errors.Length, Is.EqualTo(expectedErrors));
    }

    [Test]
    public void GetAntiFraudHeaders() {
      var af = new AntiFraud.AntiFraud() {
        ConnectionMethod = ConnectionMethod.DESKTOP_APP_DIRECT,
        DeviceID = $"{Guid.NewGuid()}",
        Screens = [new Screen(1920, 1080, 32, 1)],
        TimeZone = TimeZoneInfo.Local,
        VendorVersion = new Dictionary<string, string>() { { "TipsTrade.HMRC.Tests", "0.0.0.1" } },
        WindowSize = new Size(1024, 768)
      };

      af.PopulateLocalIPs();
      af.PopulateMACAddresses();
      af.PopulateUserAgent();

      var headers = af.GetAntiFraudHeaders();

    }

    [Test]
    public void GetPropertiesForMethod() {
      var props = AntiFraud.AntiFraud.GetPropertiesForMethod(ConnectionMethod.DESKTOP_APP_DIRECT);
      Assert.That(props.Count(), Is.EqualTo(14));
    }
  }
}
