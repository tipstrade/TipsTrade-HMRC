using TipsTrade.HMRC.Api;
using TipsTrade.HMRC.Api.Vat;
using NUnit.Framework;
using System.Threading.Tasks;

namespace TipsTrade.HMRC.Tests {
  public class VatNumberTests : TestBase {
    [Test]
    public async Task EmptyValidNumber() {
      var svc = GetService<VatNumberService>();
      var action = () => svc.CheckVrnAsync("");

      await Assert.ThatAsync(action, Throws.ArgumentException);
    }

    [Test]
    public async Task InvalidNumber() {
      var svc = GetService<VatNumberService>();
      var action = () => svc.CheckVrnAsync("000000000");

      await Assert.ThatAsync(action, Throws.TypeOf<ApiException>());
    }

    [Test]
    public async Task ValidNumber() {
      var svc = GetService<VatNumberService>();

      var resp = await svc.CheckVrnAsync("553557881");

      // {"target":{"name":"Credite Sberger Donal Inc.","vatNumber":"553557881","address":{"line1":"131B Barton Hamlet","postcode":"SW97 5CK","countryCode":"GB"}},"processingDate":"2024-09-03T09:56:20+01:00"}
      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.Target, Is.Not.Null);
      Assert.That(resp.Target.Name, Is.EqualTo("Credite Sberger Donal Inc."));
      Assert.That(resp.Target.VatNumber, Is.EqualTo("553557881"));
      Assert.That(resp.Target.Address, Is.Not.Null);
      Assert.That(resp.Target.Address.Line1, Is.EqualTo("131B Barton Hamlet"));
      Assert.That(resp.Target.Address.Postcode, Is.EqualTo("SW97 5CK"));
      Assert.That(resp.Target.Address.CountryCode, Is.EqualTo("GB"));
    }

    [Test]
    public async Task ValidNumberVerified() {
      var svc = GetService<VatNumberService>();

      var resp = await svc.CheckVrnAsync("553557881", "146295999727");

      Assert.That(resp, Is.Not.Null);
      Assert.That(resp.ConsultationNumber, Is.Not.Empty);
      Assert.That(resp.Requester, Is.EqualTo("146295999727"));
      Assert.That(resp.Target, Is.Not.Null);
      Assert.That(resp.Target.Name, Is.EqualTo("Credite Sberger Donal Inc."));
      Assert.That(resp.Target.VatNumber, Is.EqualTo("553557881"));
      Assert.That(resp.Target.Address, Is.Not.Null);
      Assert.That(resp.Target.Address.Line1, Is.EqualTo("131B Barton Hamlet"));
      Assert.That(resp.Target.Address.Postcode, Is.EqualTo("SW97 5CK"));
      Assert.That(resp.Target.Address.CountryCode, Is.EqualTo("GB"));
    }
  }
}
