using Newtonsoft.Json;
using System;
using TipsTrade.HMRC.Api.TestFraudPrevention.Model;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TipsTrade.HMRC.Tests {
  public class FraudPreventionTests : TestBase {
    public FraudPreventionTests(ITestOutputHelper output) : base(output) { }

    /// <summary>
    /// Tests that the GetFeedback method of the TestFraudPrevention API returns a valid response for all services.
    /// </summary>
    /// <remarks>
    /// Many services are not implemented in the sandbox. Each InlineData now includes a boolean that indicates
    /// whether the API is implemented. Unimplemented cases will be reported as skipped in Test Explorer.
    /// </remarks>
    [SkippableTheory]
    [InlineData("business-details-mtd", true)]
    [InlineData("business-income-source-summary-mtd", false)]
    [InlineData("business-source-adjustable-summary-mtd", false)]
    [InlineData("cis-deductions-mtd", false)]
    [InlineData("individual-calculations-mtd", true)]
    [InlineData("individual-losses-mtd", false)]
    [InlineData("individuals-business-end-of-period-statement-mtd", false)]
    [InlineData("individuals-charges-mtd", false)]
    [InlineData("individuals-disclosures-mtd", false)]
    [InlineData("individuals-expenses-mtd", false)]
    [InlineData("individuals-income-received-mtd", false)]
    [InlineData("individuals-reliefs-mtd", false)]
    [InlineData("individuals-state-benefits-mtd", false)]
    [InlineData("obligations-mtd", true)]
    [InlineData("other-deductions-mtd", false)]
    [InlineData("property-business-mtd", false)]
    [InlineData("self-assessment-mtd", false)]
    [InlineData("self-assessment-accounts-mtd", false)]
    [InlineData("self-assessment-assist-mtd", false)]
    [InlineData("self-employment-business-mtd", true)]
    [InlineData("vat-mtd", true)]
    [InlineData("individuals-capital-gains-income-mtd", false)]
    [InlineData("individuals-dividends-income-mtd", false)]
    [InlineData("individuals-employments-income-mtd", false)]
    [InlineData("individuals-foreign-income-mtd", false)]
    [InlineData("individuals-insurance-policies-income-mtd", false)]
    [InlineData("individuals-other-income-mtd", false)]
    [InlineData("individuals-pensions-income-mtd", false)]
    [InlineData("individuals-savings-income-mtd", false)]
    [InlineData("self-assessment-individual-details-mtd", false)]
    public void GetFeedback(string service, bool implemented) {
      Skip.IfNot(implemented, $"API for service '{service}' is not implemented in the sandbox.");

      var client = GetClient();
      var response = client.TestFraudPrevention.GetFeedback(service, AntiFraud.ConnectionMethod.BATCH_PROCESS_DIRECT);

      Assert.NotNull(response);
      Assert.NotNull(response.Requests);
      Assert.NotEmpty(response.Requests);
      Assert.False(response.HasErrors());

      Output.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
    }

    [Fact]
    public void Validate() {
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
