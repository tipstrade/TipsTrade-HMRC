using System;
using TipsTrade.HMRC.Api;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class VatNumberTests : TestBase {
    public VatNumberTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void TestEmptyValidNumber() {
      var client = GetClient();

      Assert.Throws<ArgumentException>(() => client.VatNumber.CheckVrn(""));
    }

    [Fact]
    public void TestInvalidNumber() {
      var client = GetClient();

      Assert.Throws<ApiException>(() => client.VatNumber.CheckVrn("000000000"));
    }

    [Fact]
    public void TestValidNumber() {
      var client = GetClient();

      var resp = client.VatNumber.CheckVrn("553557881");

      // {"target":{"name":"Credite Sberger Donal Inc.","vatNumber":"553557881","address":{"line1":"131B Barton Hamlet","postcode":"SW97 5CK","countryCode":"GB"}},"processingDate":"2024-09-03T09:56:20+01:00"}
      Assert.NotNull(resp);
      Assert.Equal("Credite Sberger Donal Inc.", resp.Target.Name);
      Assert.Equal("553557881", resp.Target.VatNumber);
      Assert.Equal("131B Barton Hamlet", resp.Target.Address.Line1);
      Assert.Equal("SW97 5CK", resp.Target.Address.Postcode);
      Assert.Equal("GB", resp.Target.Address.CountryCode);
    }

    [Fact]
    public void TestValidNumberVerified() {
      var client = GetClient();

      var resp = client.VatNumber.CheckVrn("553557881", "146295999727");

      Assert.NotNull(resp);
      Assert.NotEmpty(resp.ConsultationNumber);
      Assert.Equal("146295999727", resp.Requester);
      Assert.Equal("Credite Sberger Donal Inc.", resp.Target.Name);
      Assert.Equal("553557881", resp.Target.VatNumber);
      Assert.Equal("131B Barton Hamlet", resp.Target.Address.Line1);
      Assert.Equal("SW97 5CK", resp.Target.Address.Postcode);
      Assert.Equal("GB", resp.Target.Address.CountryCode);
    }
  }
}
