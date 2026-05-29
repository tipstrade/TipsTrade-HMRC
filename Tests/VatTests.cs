using Newtonsoft.Json;
using System;
using System.Linq;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Vat;
using TipsTrade.HMRC.Api.Vat.Model;
using NUnit.Framework;

namespace TipsTrade.HMRC.Tests {
  public class VatTests : TestBase {
    public VatTests() {
    }

    [SetUp]
    protected override void CustomSetup() {
      SetupCredentialsForOrganisation();
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
    public void GetReturn() {
      var obRequest = new ObligationsRequest() {
        Vrn = Users.Organisation.User.Vrn,
      };
      PopulateDateRange(obRequest);

      var svc = GetService<VatService>();

      var obligations = svc.GetObligations(obRequest);
      var periodKey = obligations.Value.Where(o => !o.IsOpen).LastOrDefault().PeriodKey;

      var returnRequest = new ReturnRequest() {
        Vrn = Users.Organisation.User.Vrn,
        PeriodKey = periodKey
      };

      var resp = svc.GetReturn(returnRequest);
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

      TestContext.Progress.WriteLine("VAT Retrieved return:");
      TestContext.Progress.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }

    [Test]
    public void Liabilities() {
      var request = new LiabilitiesRequest() {
        GovTestScenario = LiabilitiesRequest.ScenarioMultipleLiabilities,
        DateFrom = new DateTime(2017, 2, 27),
        DateTo = new DateTime(2017, 12, 31),
        Vrn = Users.Organisation.User.Vrn,
      };

      var svc = GetService<VatService>();

      var resp = svc.GetLiabilities(request);
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

      TestContext.Progress.WriteLine("VAT Liabilities");
      TestContext.Progress.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }

    [Test]
    public void Obligations() {
      var obligations = new ObligationsRequest() {
        GovTestScenario = ObligationsRequest.ScenarioMonthlylyMet2,
        Vrn = Users.Organisation.User.Vrn,
      };

      PopulateDateRange(obligations);

      var svc = GetService<VatService>();

      ObligationResponse resp;

      // All, expect only two to be fulfilled
      resp = svc.GetObligations(obligations);
      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);
      Assert.That(resp.Value.Where(x => x.IsFulfilled).Count(), Is.EqualTo(2));
      foreach (var item in resp.Value) {
        AssertExtensions.NotDefault(item.Start);
        AssertExtensions.NotDefault(item.End);
        AssertExtensions.NotDefault(item.Due);
        Assert.That(item.PeriodKey, Is.Not.Null);
      }

      TestContext.Progress.WriteLine("VAT Obligations");
      TestContext.Progress.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));

      // Fulfulled
      obligations.Status = "F";
      obligations.GovTestScenario = null;
      resp = svc.GetObligations(obligations);
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
      resp = svc.GetObligations(obligations);
      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);
      foreach (var item in resp.Value) {
        Assert.That(item.Status, Is.EqualTo("O"));
        Assert.That(item.IsOpen, Is.True);
        Assert.That(item.Received, Is.Null);
      }
    }

    [Test]
    public void Payments() {
      var request = new PaymentsRequest() {
        GovTestScenario = PaymentsRequest.ScenarioMultiplePayment,
        DateFrom = new DateTime(2017, 2, 27),
        DateTo = new DateTime(2017, 12, 31),
        Vrn = Users.Organisation.User.Vrn,
      };

      var svc = GetService<VatService>();

      var resp = svc.GetPayments(request);
      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Value, Is.Not.Empty);

      foreach (var item in resp.Value) {
        AssertExtensions.NotDefault(item.Amount);
        if (item.Received != null) {
          AssertExtensions.NotDefault(item.Received);
        }
      }

      TestContext.Progress.WriteLine("VAT Payments");
      TestContext.Progress.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
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
    public void Submission() {
      var obRequest = new ObligationsRequest() {
        Vrn = Users.Organisation.User.Vrn,
      };
      PopulateDateRange(obRequest);

      var svc = GetService<VatService>();

      var obligations = svc.GetObligations(obRequest);
      var periodKey = obligations.Value.Where(o => o.IsOpen).LastOrDefault().PeriodKey;

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
        Vrn = Users.Organisation.User.Vrn,
        GovTestScenario = SubmitRequest.ScenarioDuplicateSubmission
      };

      var ex = Assert.Throws<Api.ApiException>((Action)(() => svc.SubmitReturn(request)));
      Assert.That(ex.Message, Is.EqualTo("Business validation error"));
      Assert.That(ex.ApiError.Code, Is.EqualTo("BUSINESS_ERROR"));
      Assert.That(ex.ApiError.Message, Is.EqualTo("Business validation error"));
      Assert.That(ex.ApiError.Errors, Has.Length.EqualTo(1));
      Assert.That(ex.ApiError.Errors.First().Code, Is.EqualTo("DUPLICATE_SUBMISSION"));
      Assert.That(ex.ApiError.Errors.First().Message, Is.EqualTo("The VAT return was already submitted for the given period."));

      request.GovTestScenario = null;

      var resp = svc.SubmitReturn(request);

      TestContext.Progress.WriteLine("VAT Submission:");
      TestContext.Progress.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }
  }
}
