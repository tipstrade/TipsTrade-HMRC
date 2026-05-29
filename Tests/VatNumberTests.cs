using System;
using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.Vat;
using Xunit;
using Xunit.Abstractions;

namespace TipsTrade.HMRC.Tests {
  public class VatNumberTests : TestBase {
    public VatNumberTests(ITestOutputHelper output) : base(output) {
    }

    [Fact]
    public void EmptyValidNumber() {
      var svc = GetService<VatNumberService>();

      Assert.Throws<ArgumentException>(() => svc.CheckVrn(""));
    }

    [Fact]
    public void InvalidNumber() {
      var svc = GetService<VatNumberService>();

      Assert.Throws<ApiException>(() => svc.CheckVrn("000000000"));
    }

    [Fact]
    public void ValidNumber() {
      var svc = GetService<VatNumberService>();

      var resp = svc.CheckVrn("553557881");

      // {"target":{"name":"Credite Sberger Donal Inc.","vatNumber":"553557881","address":{"line1":"131B Barton Hamlet","postcode":"SW97 5CK","countryCode":"GB"}},"processingDate":"2024-09-03T09:56:20+01:00"}
      Assert.NotNull(resp);
      Assert.Equal("Credite Sberger Donal Inc.", resp.Target.Name);
      Assert.Equal("553557881", resp.Target.VatNumber);
      Assert.Equal("131B Barton Hamlet", resp.Target.Address.Line1);
      Assert.Equal("SW97 5CK", resp.Target.Address.Postcode);
      Assert.Equal("GB", resp.Target.Address.CountryCode);
    }

    [Fact]
    public void ValidNumberVerified() {
      var svc = GetService<VatNumberService>();

      var resp = svc.CheckVrn("553557881", "146295999727");

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
