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
      var obligations = new ObligationsRequest() {
        Vrn = OrganisationUser.Vrn,
        From = new DateTime(2018, 1, 1),
        To = new DateTime(2018, 11, 1).AddDays(-1),
        Status = ObligationStatus.Fulfilled
      };

      var client = Client;
      client.AccessToken = AccessToken;

      var resp = client.Vat.GetObligations(obligations);
    }
  }
}
