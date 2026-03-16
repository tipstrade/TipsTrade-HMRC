using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class FraudPreventionTests : TestBase {
    public FraudPreventionTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void FraudPrevention() {
      var client = GetClient();
      var response = client.TestFraudPrevention.Validate();

      Assert.Empty(response.Errors);
      Assert.Empty(response.Warnings); // Warnings may be present if the dev machine has a VPN or unusual network configuration.

      Output.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
    }

    [Fact]
    public void PopulateLocalIPs_Predicate_Is_Called() {
      var client = GetClient();

      bool isCalled = false;
      Func<System.Net.IPAddress, bool> func = (ip) => {
        isCalled = true;
        return true;
      };

      client.AntiFraud.PopulateLocalIPs(func);

      Assert.True(isCalled);
    }
  }
}
