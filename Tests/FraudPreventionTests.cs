using Newtonsoft.Json;
using System;
using TipsTrade.HMRC.Api.TestFraudPrevention;
using TipsTrade.HMRC.Api.TestFraudPrevention.Model;
using NUnit.Framework;
using TipsTrade.HMRC.FraudPrevention.Headers;
using TipsTrade.HMRC.FraudPrevention.ConnectionMethods;
using System.Threading.Tasks;
using Moq;
using TipsTrade.HMRC.FraudPrevention;
using System.Linq;

namespace TipsTrade.HMRC.Tests {
  public class FraudPreventionTests : TestBase {
    /// <summary>
    /// Tests that the GetFeedback method of the TestFraudPrevention API returns a valid response for all services.
    /// </summary>
    /// <remarks>
    /// Many services are not implemented in the sandbox. Each InlineData now includes a boolean that indicates
    /// whether the API is implemented. Unimplemented cases will be reported as skipped in Test Explorer.
    /// </remarks>
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
    public async Task GetFeedback(string service, bool implemented) {
      Assume.That(implemented, Is.True, $"API for service '{service}' is not implemented in the sandbox.");

      var svc = GetService<TestFraudPreventionService>();
      var response = await svc.GetFeedbackAsync(service, ConnectionMethod.BATCH_PROCESS_DIRECT);

      Assert.That(response, Is.Not.Null);
      Assert.That(response.Requests, Is.Not.Null);
      Assert.That(response.Requests, Is.Not.Empty);
      Assert.That(response.HasErrors(), Is.False);

      // Log the times each endpoint was called for the service, along with the method and path. This can help identify if certain endpoints are being called more frequently than expected.
      var lastTimes = response.Requests.Select(x => $"{x.RequestTimestamp} {x.Method} {x.Path}");

      TestContext.Out.WriteLine($"Feedback for service '{service}':");
      TestContext.Out.WriteLine(string.Join(Environment.NewLine, lastTimes));

      TestContext.Out.WriteLine();
      TestContext.Out.WriteLine("Full response:");
      TestContext.Out.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
    }

    [TestCase(typeof(BatchProcessDirect))]
    [TestCase(typeof(DesktopAppDirect))]
    [TestCase(typeof(DesktopAppViaServer))]
    [TestCase(typeof(MobileAppDirect))]
    [TestCase(typeof(MobileAppViaServer))]
    [TestCase(typeof(OtherDirect))]
    [TestCase(typeof(OtherViaServer))]
    [TestCase(typeof(WebAppViaServer))]
    public async Task Validate(Type headersType) {
      // Build the headers for the test case type and populate
      IFraudPrevention value = (IFraudPrevention?)Activator.CreateInstance(headersType) ?? throw new InvalidOperationException($"Failed to create instance of {headersType.Name}");
      PopulateFraudPrevention(value);

      var headers = value.GetHeaders().ToArray();
      var headerKeys = headers.Select(h => h.Name).Distinct().ToArray();

      // Ensure that no headers are duplicated and that the keys are all valid non-empty strings.
      using (Assert.EnterMultipleScope()) {
        Assert.That(headerKeys, Has.Length.EqualTo(headers.Length), "Duplicate headers found.");
        Assert.That(headerKeys.Any(string.IsNullOrEmpty), Is.False, "Header keys contain null or empty strings.");
      }

      // Inject the headers in the HmrcOptionsMock
      HmrcOptionsMock.Reset();
      HmrcOptionsMock.Setup(x => x.Value).Returns(new HmrcOptions {
        FraudPreventionConfig = value,
        ClientId = ClientId,
        ClientSecret = ClientSecret,
        IsSandbox = IsSandbox
      });

      TestContext.Out.WriteLine($"Testing {headersType.Name} headers:");
      TestContext.Out.WriteLine(JsonConvert.SerializeObject(value.GetHeaders().ToDictionary(x => x.Name, x => x.Value), Formatting.Indented));

      var svc = GetService<TestFraudPreventionService>();
      var response = await svc.ValidateAsync();

      TestContext.Out.WriteLine();
      TestContext.Out.WriteLine("Response from ValidateAsync:");
      TestContext.Out.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));

      using (Assert.EnterMultipleScope()) {
        Assert.That(response.Errors, Is.Empty);
        Assert.That(response.Warnings, Is.Empty); // Warnings may be present if the dev machine has a VPN or unusual network configuration.
      }
    }

    [Test]
    public void PopulateLocalIPs_Predicate_Is_Called() {
      var headers = BuildFraudPrevention<BatchProcessDirect>();

      bool isCalled = false;
      Func<System.Net.IPAddress, bool> func = (ip) => {
        isCalled = true;
        return true;
      };

      headers.PopulateLocalIps(func);

      Assert.That(isCalled, Is.True);
    }

    [Test]
    public void UserAgent_Populate_Works() {
      var headers = new DesktopAppDirect();

      var nullManufacturerAction = () => headers.PopulateUserAgent(null!, "XPS");
      var nullModelAction = () => headers.PopulateUserAgent("Dell", null!);
      ArgumentNullException ex;

      ex = Assert.Throws<ArgumentNullException>(nullManufacturerAction);
      Assert.That(ex.ParamName, Is.EqualTo("deviceManufacturer"));

      ex = Assert.Throws<ArgumentNullException>(nullModelAction);
      Assert.That(ex.ParamName, Is.EqualTo("deviceModel"));

      headers.PopulateUserAgent("Dell", "XPS");

      Assert.That(headers.UserAgent, Is.Not.Null);
      Assert.That(headers.UserAgent.OSFamily, Is.Not.Empty);
      Assert.That(headers.UserAgent.OSVersion, Is.Not.Empty);
      Assert.That(headers.UserAgent.DeviceManufacturer, Is.EqualTo("Dell"));
      Assert.That(headers.UserAgent.DeviceModel, Is.EqualTo("XPS"));
    }
  }
}
