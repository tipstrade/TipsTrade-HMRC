using Newtonsoft.Json;
using System;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Vat.Model;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class VatTests : TestBase {
    public VatTests(ITestOutputHelper output) : base(output) {
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
    public void TestObligations() {
      var obligations = new ObligationsRequest() {
        Vrn = OrganisationUser.Vrn,
      };

      PopulateDateRange(obligations);

      var client = Client;
      client.AccessToken = AccessToken;

      ObligationResult[] resp;

      // All
      resp = client.Vat.GetObligations(obligations);
      Assert.NotNull(resp);
      Assert.NotEmpty(resp);
      foreach (var item in resp) {
        Assert.NotEqual(default(DateTime), item.Start);
        Assert.NotEqual(default(DateTime), item.End);
        Assert.NotEqual(default(DateTime), item.Due);
        Assert.NotNull(item.PeriodKey);
      }

      Output.WriteLine("VAT Obligations");
      Output.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));

      // Fulfulled
      obligations.Status = ObligationStatus.Fulfilled;
      resp = client.Vat.GetObligations(obligations);
      Assert.NotNull(resp);
      Assert.NotEmpty(resp);
      foreach (var item in resp) {
        Assert.Equal(ObligationStatus.Fulfilled, item.Status);
        Assert.NotNull(item.Received);
      }

      // Open
      obligations.Status = ObligationStatus.Open;
      resp = client.Vat.GetObligations(obligations);
      Assert.NotNull(resp);
      Assert.NotEmpty(resp);
      foreach (var item in resp) {
        Assert.Equal(ObligationStatus.Open, item.Status);
        Assert.Null(item.Received);
      }
    }

    [Fact]
    public void TestPayments() {
      var request = new DateRangeRequest() {
        GovTestScenario = "MULTIPLE_PAYMENTS",
        Vrn = OrganisationUser.Vrn,
      };

      request.From = new DateTime(2017, 2, 27);
      request.To = new DateTime(2017, 12, 31);

      var client = Client;
      client.AccessToken = AccessToken;

      var resp = client.Vat.GetPayments(request);
      Assert.NotNull(resp);
      Assert.NotEmpty(resp);

      foreach (var item in resp) {
        Assert.NotEqual(default(decimal), item.Amount);
        if (item.Received != null) {
          Assert.NotEqual(default(DateTime), item.Received);
        }
      }

      Output.WriteLine("VAT Payments");
      Output.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }
  }
}
