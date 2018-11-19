using Newtonsoft.Json;
using System;
using TipsTrade.HMRC.Api.Vat.Model;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class VatTests : TestBase {
    public VatTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void TestObligations() {
      var year = DateTime.Now.Year;
      if (DateTime.Now.Month < 3) {
        year--;
      }

      var obligations = new ObligationsRequest() {
        Vrn = OrganisationUser.Vrn,
        From = new DateTime(year, 1, 1),
      };
      obligations.To = obligations.From.AddYears(1).AddDays(-1);

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
  }
}
