using Newtonsoft.Json;
using System;
using System.Linq;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Vat;
using TipsTrade.HMRC.Api.Vat.Model;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class VatTests : TestBase {
    public VatTests(ITestOutputHelper output) : base(output) {
    }

    /// <summary>This is the VAT return used for testing submissions and retrievals.</summary>
    private VatReturn CreateVatReturn(string periodKey) {
      return new VatReturn() {
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
      };
    }

    private void PopulateDateRange(IDateRange value) {
      var year = DateTime.Now.Year;
      if (DateTime.Now.Month < 3) {
        year--;
      }

      value.From = new DateTime(year, 1, 1);
      value.To = value.From.AddYears(1).AddDays(-1);
    }

    [Fact]
    public void TestGetReturn() {
      var obRequest = new ObligationsRequest() {
        Vrn = Users.Organisation.User.Vrn,
      };
      PopulateDateRange(obRequest);

      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var obligations = client.Vat.GetObligations(obRequest);
      var periodKey = obligations.Value.Where(o => o.Status == ObligationStatus.Open).LastOrDefault().PeriodKey;

      var returnRequest = new ReturnRequest() {
        Vrn = Users.Organisation.User.Vrn,
        PeriodKey = periodKey
      };

      var resp = client.Vat.GetReturn(returnRequest);
      Assert.NotNull(resp);

      // Compare to the expected return
      var expected = CreateVatReturn(periodKey);
      foreach (var prop in expected.GetType().GetProperties()) {
        object expectedValue = prop.GetValue(expected);

        var returnedProp = typeof(ReturnResponse).GetProperty(prop.Name);
        object returnedValue = returnedProp.GetValue(resp);

        if (prop.Name == nameof(VatReturn.Finalised)) {
          Assert.Null(returnedValue); // Finalised isn't returned by GetReturn
        } else {
          Assert.Equal(expectedValue, returnedValue);
        }
      }

      Output.WriteLine("VAT Retrieved return:");
      Output.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }

    [Fact]
    public void TestLiabilities() {
      var request = new LiabilitiesRequest() {
        GovTestScenario = LiabilitiesRequest.ScenarioMultipleLiabilities,
        From = new DateTime(2017, 2, 27),
        To = new DateTime(2017, 12, 31),
        Vrn = Users.Organisation.User.Vrn,
      };

      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var resp = client.Vat.GetLiabilities(request);
      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);

      foreach (var item in resp.Value) {
        Assert.NotNull(item.TaxPeriod);
        Assert.NotDefault(item.TaxPeriod.From);
        Assert.NotDefault(item.TaxPeriod.To);
        Assert.NotNull(item.Type);
        Assert.NotDefault(item.OriginalAmount);
        if (item.Due != null) {
          Assert.NotDefault(item.Due);
        }
      }

      Output.WriteLine("VAT Liabilities");
      Output.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }

    [Fact]
    public void TestObligations() {
      var obligations = new ObligationsRequest() {
        GovTestScenario = ObligationsRequest.ScenarioMonthlylyMet2,
        Vrn = Users.Organisation.User.Vrn,
      };

      PopulateDateRange(obligations);

      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      ObligationResponse resp;

      // All, expect only two to be fulfilled
      resp = client.Vat.GetObligations(obligations);
      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);
      Assert.Equal(2, resp.Value.Where(x => x.Status == ObligationStatus.Fulfilled).Count());
      foreach (var item in resp.Value) {
        Assert.NotDefault(item.Start);
        Assert.NotDefault(item.End);
        Assert.NotDefault(item.Due);
        Assert.NotNull(item.PeriodKey);
      }

      Output.WriteLine("VAT Obligations");
      Output.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));

      // Fulfulled
      obligations.Status = ObligationStatus.Fulfilled;
      obligations.GovTestScenario = null;
      resp = client.Vat.GetObligations(obligations);
      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);
      foreach (var item in resp.Value) {
        Assert.Equal(ObligationStatus.Fulfilled, item.Status);
        Assert.NotNull(item.Received);
      }

      // Open
      obligations.Status = ObligationStatus.Open;
      obligations.GovTestScenario = null;
      resp = client.Vat.GetObligations(obligations);
      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);
      foreach (var item in resp.Value) {
        Assert.Equal(ObligationStatus.Open, item.Status);
        Assert.Null(item.Received);
      }
    }

    [Fact]
    public void TestPayments() {
      var request = new PaymentsRequest() {
        GovTestScenario = PaymentsRequest.ScenarioMultiplePayment,
        From = new DateTime(2017, 2, 27),
        To = new DateTime(2017, 12, 31),
        Vrn = Users.Organisation.User.Vrn,
      };

      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var resp = client.Vat.GetPayments(request);
      Assert.NotNull(resp);
      Assert.NotEmpty(resp.Value);

      foreach (var item in resp.Value) {
        Assert.NotDefault(item.Amount);
        if (item.Received != null) {
          Assert.NotDefault(item.Received);
        }
      }

      Output.WriteLine("VAT Payments");
      Output.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }

    [Fact]
    public void TestReturnSerialization() {
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

      Assert.Equal("#001", resp.PeriodKey);
      Assert.Equal(7724.92M, resp.VatDueSales);
      Assert.Equal(100.00M, resp.VatDueAcquisitions);
      Assert.Equal(7824.92M, resp.TotalVatDue);
      Assert.Equal(1681.08M, resp.VatReclaimedCurrPeriod);
      Assert.Equal(6143.84M, resp.NetVatDue);
      Assert.Equal(38622M, resp.TotalValueSalesExVAT);
      Assert.Equal(8405M, resp.TotalValuePurchasesExVAT);
      Assert.Equal(200M, resp.TotalValueGoodsSuppliedExVAT);
      Assert.Equal(300M, resp.TotalAcquisitionsExVAT);
    }

    [Fact]
    public void TestSubmission() {
      var obRequest = new ObligationsRequest() {
        Vrn = Users.Organisation.User.Vrn,
      };
      PopulateDateRange(obRequest);

      var client = GetClient();
      client.AccessToken = Users.Organisation.Tokens.AccessToken;

      var obligations = client.Vat.GetObligations(obRequest);
      var periodKey = obligations.Value.Where(o => o.Status == ObligationStatus.Open).LastOrDefault().PeriodKey;

      var request = new SubmitRequest() {
        Return = CreateVatReturn(periodKey),
        Vrn = Users.Organisation.User.Vrn
      };

      var resp = client.Vat.SubmitReturn(request);

      Output.WriteLine("VAT Submission:");
      Output.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }
  }
}
