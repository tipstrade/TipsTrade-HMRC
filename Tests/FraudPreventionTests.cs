using Newtonsoft.Json;
using System;
using TipsTrade.HMRC.Api.TestFraudPrevention;
using TipsTrade.HMRC.Api.TestFraudPrevention.Model;
using NUnit.Framework;

namespace TipsTrade.HMRC.Tests {
  public class FraudPreventionTests : TestBase {
    /// <summary>
    /// Tests that the GetFeedback method of the TestFraudPrevention API returns a valid response for all services.
    /// </summary>
    /// <remarks>
    /// Many services are not implemented in the sandbox. Each InlineData now includes a boolean that indicates
    /// whether the API is implemented. Unimplemented cases will be reported as skipped in Test Explorer.
    /// </remarks>
    [Test]
    [TestCase("business-details-mtd", true)]
    [TestCase("business-income-source-summary-mtd", false)]
    [TestCase("business-source-adjustable-summary-mtd", false)]
    [TestCase("cis-deductions-mtd", false)]
    [TestCase("individual-calculations-mtd", true)]
    [TestCase("individual-losses-mtd", false)]
    [TestCase("individuals-business-end-of-period-statement-mtd", false)]
    [TestCase("individuals-charges-mtd", false)]
    [TestCase("individuals-disclosures-mtd", false)]
    [TestCase("individuals-expenses-mtd", false)]
    [TestCase("individuals-income-received-mtd", false)]
    [TestCase("individuals-reliefs-mtd", false)]
    [TestCase("individuals-state-benefits-mtd", false)]
    [TestCase("obligations-mtd", true)]
    [TestCase("other-deductions-mtd", false)]
    [TestCase("property-business-mtd", false)]
    [TestCase("self-assessment-mtd", false)]
    [TestCase("self-assessment-accounts-mtd", false)]
    [TestCase("self-assessment-assist-mtd", false)]
    [TestCase("self-employment-business-mtd", true)]
    [TestCase("vat-mtd", true)]
    [TestCase("individuals-capital-gains-income-mtd", false)]
    [TestCase("individuals-dividends-income-mtd", false)]
    [TestCase("individuals-employments-income-mtd", false)]
    [TestCase("individuals-foreign-income-mtd", false)]
    [TestCase("individuals-insurance-policies-income-mtd", false)]
    [TestCase("individuals-other-income-mtd", false)]
    [TestCase("individuals-pensions-income-mtd", false)]
    [TestCase("individuals-savings-income-mtd", false)]
    [TestCase("self-assessment-individual-details-mtd", false)]
    public void GetFeedback(string service, bool implemented) {
      Assume.That(implemented, Is.True, $"API for service '{service}' is not implemented in the sandbox.");

      var svc = GetService<TestFraudPreventionService>();
      var response = svc.GetFeedback(service, AntiFraud.ConnectionMethod.BATCH_PROCESS_DIRECT);

      Assert.That(response, Is.Not.Null);
      Assert.That(response.Requests, Is.Not.Null);
      Assert.That(response.Requests, Is.Not.Empty);
      Assert.That(response.HasErrors(), Is.False);

      TestContext.Out.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
    }

    [Test]
    public void Validate() {
      var svc = GetService<TestFraudPreventionService>();
      var response = svc.Validate();

      Assert.That(response.Errors, Is.Empty);
      Assert.That(response.Warnings, Is.Empty); // Warnings may be present if the dev machine has a VPN or unusual network configuration.

      TestContext.Out.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
    }

    [Test]
    public void PopulateLocalIPs_Predicate_Is_Called() {
      var antiFraud = BuildAntiFraud();

      bool isCalled = false;
      Func<System.Net.IPAddress, bool> func = (ip) => {
        isCalled = true;
        return true;
      };

      antiFraud.PopulateLocalIPs(func);

      Assert.That(isCalled, Is.True);
    }
  }
}
