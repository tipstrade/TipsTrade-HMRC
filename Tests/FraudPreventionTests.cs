using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TipsTrade.HMRC.AntiFraud;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class FraudPreventionTests : TestBase {
    public FraudPreventionTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void TestFraudPrevention() {
      var client = GetClient();
      var response = client.TestFraudPrevention.Validate();

      Assert.Empty(response.Errors);
      Assert.Empty(response.Warnings);

      Output.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
    }
  }
}
