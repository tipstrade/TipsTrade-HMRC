using Newtonsoft.Json;
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
      Assert.Empty(response.Warnings);

      Output.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
    }
  }
}
