using Newtonsoft.Json;
using System;
using System.Linq;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Vat;
using TipsTrade.HMRC.Api.Vat.Model;
using NUnit.Framework;
using System.Threading.Tasks;

namespace TipsTrade.HMRC.Tests {
  public class VatTests : TestBase {
    protected override void CustomSetup() {
      SetupCredentialsForOrganisation();
    }

    private string GetVrn() {
      return Users?.Organisation?.User?.Vrn ?? throw new InvalidOperationException("VRN is not set for the user.");
    }

    private void PopulateDateRange(IDateRange value) {
      var year = DateTime.Now.Year;
      if (DateTime.Now.Month < 3) {
        year--;
      }

      value.DateFrom = new DateTime(year, 1, 1);
      value.DateTo = value.DateFrom.AddYears(1).AddDays(-1);
    }

    [Test, Ignore("Ignored as the sandbox doesn't return returns outside of 4 years.")]
    public async Task GetReturn() {
      var obRequest = new ObligationsRequest() {
        Vrn = GetVrn(),
      };
      PopulateDateRange(obRequest);

      var svc = GetService<VatService>();

      var obligations = await svc.GetObligationsAsync(obRequest);
      var periodKey = obligations.Value?.Where(o => !o.IsOpen).LastOrDefault()?.PeriodKey;
      Assert.That(periodKey, Is.Not.Null, "No fulfilled obligations found to test with.");

      var returnRequest = new ReturnRequest() {
        Vrn = GetVrn(),
        PeriodKey = periodKey
      };

      var resp = await svc.GetReturnAsync(returnRequest);

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.PeriodKey, Is.EqualTo(periodKey));
      AssertExtensions.NotDefault(resp.VatDueSales);
      AssertExtensions.NotDefault(resp.VatDueAcquisitions);
      AssertExtensions.NotDefault(resp.TotalVatDue);
      AssertExtensions.NotDefault(resp.VatReclaimedCurrPeriod);
      AssertExtensions.NotDefault(resp.NetVatDue);
      AssertExtensions.NotDefault(resp.TotalValueSalesExVAT);
      AssertExtensions.NotDefault(resp.TotalValuePurchasesExVAT);
      AssertExtensions.NotDefault(resp.TotalValueGoodsSuppliedExVAT);
      AssertExtensions.NotDefault(resp.TotalAcquisitionsExVAT);
      AssertExtensions.NotDefault(resp.Finalised);

      TestContext.Out.WriteLine("VAT Retrieved return:");
      TestContext.Out.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }

    [Test]
    public async Task Liabilities() {
      var request = new LiabilitiesRequest() {
        GovTestScenario = LiabilitiesRequest.ScenarioMultipleLiabilities,
        DateFrom = new DateTime(2017, 2, 27),
        DateTo = new DateTime(2017, 12, 31),
        Vrn = GetVrn(),
      };

      var svc = GetService<VatService>();

      var resp = await svc.GetLiabilitiesAsync(request);
      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);

      foreach (var item in resp.Value) {
        Assert.That(item.TaxPeriod, Is.Not.Null);
        AssertExtensions.NotDefault(item.TaxPeriod.DateFrom);
        AssertExtensions.NotDefault(item.TaxPeriod.DateTo);
        Assert.That(item.Type, Is.Not.Null);
        AssertExtensions.NotDefault(item.OriginalAmount);
        if (item.Due != null) {
          AssertExtensions.NotDefault(item.Due);
        }
      }

      TestContext.Out.WriteLine("VAT Liabilities");
      TestContext.Out.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }

    [Test]
    public async Task Obligations() {
      var obligations = new ObligationsRequest() {
        GovTestScenario = ObligationsRequest.ScenarioMonthlylyMet2,
        Vrn = GetVrn(),
      };

      PopulateDateRange(obligations);

      var svc = GetService<VatService>();

      ObligationResponse resp;

      // All, expect only two to be fulfilled
      resp = await svc.GetObligationsAsync(obligations);
      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);
      Assert.That(resp.Value.Where(x => x.IsFulfilled).Count(), Is.EqualTo(2));
      foreach (var item in resp.Value) {
        AssertExtensions.NotDefault(item.Start);
        AssertExtensions.NotDefault(item.End);
        AssertExtensions.NotDefault(item.Due);
        Assert.That(item.PeriodKey, Is.Not.Null);
      }

      TestContext.Out.WriteLine("VAT Obligations");
      TestContext.Out.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));

      // Fulfulled
      obligations.Status = "F";
      obligations.GovTestScenario = null;
      resp = await svc.GetObligationsAsync(obligations);
      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);
      foreach (var item in resp.Value) {
        Assert.That(item.Status, Is.EqualTo("F"));
        Assert.That(item.IsFulfilled, Is.True);
        Assert.That(item.Received, Is.Not.Null);
      }

      // Open
      obligations.Status = "O";
      obligations.GovTestScenario = null;
      resp = await svc.GetObligationsAsync(obligations);
      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);
      foreach (var item in resp.Value) {
        Assert.That(item.Status, Is.EqualTo("O"));
        Assert.That(item.IsOpen, Is.True);
        Assert.That(item.Received, Is.Null);
      }
    }

    [Test]
    public async Task Payments() {
      var request = new PaymentsRequest() {
        GovTestScenario = PaymentsRequest.ScenarioMultiplePayment,
        DateFrom = new DateTime(2017, 2, 27),
        DateTo = new DateTime(2017, 12, 31),
        Vrn = GetVrn(),
      };

      var svc = GetService<VatService>();

      var resp = await svc.GetPaymentsAsync(request);
      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);

      foreach (var item in resp.Value) {
        AssertExtensions.NotDefault(item.Amount);
        if (item.Received != null) {
          AssertExtensions.NotDefault(item.Received);
        }
      }

      TestContext.Out.WriteLine("VAT Payments");
      TestContext.Out.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }

    [Test]
    public void ReturnSerialization() {
      // Taken from the docs
      var json = @"{
  ""periodKey"": ""#001"",
  ""vatDueSales"": 7724.92,
  ""vatDueAcquisitions"": 100.00,
  ""totalVatDue"": 7824.92,
  ""vatReclaimedCurrPeriod"": 1681.08,
  ""netVatDue"": 6143.84,
  ""totalValueSalesExVAT"": 38622,
  ""totalValuePurchasesExVAT"": 8405,
  ""totalValueGoodsSuppliedExVAT"": 200,
  ""totalAcquisitionsExVAT"": 300
}";

      var resp = JsonConvert.DeserializeObject<VatReturn>(json);

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.PeriodKey, Is.EqualTo("#001"));
      Assert.That(resp.VatDueSales, Is.EqualTo(7724.92M));
      Assert.That(resp.VatDueAcquisitions, Is.EqualTo(100.00M));
      Assert.That(resp.TotalVatDue, Is.EqualTo(7824.92M));
      Assert.That(resp.VatReclaimedCurrPeriod, Is.EqualTo(1681.08M));
      Assert.That(resp.NetVatDue, Is.EqualTo(6143.84M));
      Assert.That(resp.TotalValueSalesExVAT, Is.EqualTo(38622M));
      Assert.That(resp.TotalValuePurchasesExVAT, Is.EqualTo(8405M));
      Assert.That(resp.TotalValueGoodsSuppliedExVAT, Is.EqualTo(200M));
      Assert.That(resp.TotalAcquisitionsExVAT, Is.EqualTo(300M));
    }

    [Test, Ignore("The submission can only be run once.")]
    public async Task Submission() {
      var obRequest = new ObligationsRequest() {
        Vrn = GetVrn()
      };
      PopulateDateRange(obRequest);

      var svc = GetService<VatService>();

      var obligations = await svc.GetObligationsAsync(obRequest);
      var periodKey = obligations.Value?.Where(o => o.IsOpen).LastOrDefault()?.PeriodKey;
      Assert.That(periodKey, Is.Not.Null, "No open obligations found to test with.");

      var request = new SubmitRequest() {
        Return = new VatReturn() {
          PeriodKey = periodKey,
          VatDueSales = 7724.92m,
          VatDueAcquisitions = 703.49m,
          TotalVatDue = 7724.92m + 703.49m,
          VatReclaimedCurrPeriod = 1681.08m,
          NetVatDue = 7724.92m + 703.49m - 1681.08m,
          TotalValueSalesExVAT = 38622,
          TotalValuePurchasesExVAT = 8405,
          TotalValueGoodsSuppliedExVAT = 3703,
          TotalAcquisitionsExVAT = 500,
          Finalised = true
        },
        Vrn = GetVrn(),
        GovTestScenario = SubmitRequest.ScenarioDuplicateSubmission
      };

      var taskToTest = () => svc.SubmitReturnAsync(request);
      var ex = Assert.ThrowsAsync<Api.ApiException>(taskToTest);
      Assert.That(ex.Message, Is.EqualTo("Business validation error"));
      Assert.That(ex.ApiError, Is.Not.Null);
      Assert.That(ex.ApiError.Code, Is.EqualTo("BUSINESS_ERROR"));
      Assert.That(ex.ApiError.Message, Is.EqualTo("Business validation error"));
      Assert.That(ex.ApiError.Errors, Has.Length.EqualTo(1));
      Assert.That(ex.ApiError.Errors.First().Code, Is.EqualTo("DUPLICATE_SUBMISSION"));
      Assert.That(ex.ApiError.Errors.First().Message, Is.EqualTo("The VAT return was already submitted for the given period."));

      request.GovTestScenario = null;

      var resp = await svc.SubmitReturnAsync(request);

      TestContext.Out.WriteLine("VAT Submission:");
      TestContext.Out.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }
  }
}
