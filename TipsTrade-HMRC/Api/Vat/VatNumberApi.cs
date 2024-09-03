using System;
using TipsTrade.HMRC.Api.Vat.Model;

namespace TipsTrade.HMRC.Api.Vat {
  public class VatNumberApi : IApi, IClient {
    public string Description => "Check a UK VAT number API.";

    public bool IsStable => true;

    public string Location => "organisations/vat/check-vat-number";

    public string Name => "Check VAT Number API";

    public string Version => "2.0";

    public Client Client { get; set; }

    /// <summary>Verifies the specified VAT Number.</summary>
    public VatNumberCheckResponse CheckVrn(string vrn) {
      vrn = vrn?.Trim() ?? "";

      if (vrn == "") {
        throw new ArgumentException("VAT number cannot be empty.", nameof(vrn));
      }

      var restRequest = this.CreateRequest(new VatNumberCheckRequest { Vrn = vrn });

      return this.ExecuteRequest<VatNumberCheckResponse>(restRequest);
    }

    /// <summary>Verifies the specified VAT Number via a verified request.</summary>
    public VerifiedVatNumberCheckResponse CheckVrn(string vrn, string requesterVrn) {
      var restRequest = this.CreateRequest(new VerifiedVatNumberCheckRequest { Vrn = vrn, RequesterVrn = requesterVrn });

      return this.ExecuteRequest<VerifiedVatNumberCheckResponse>(restRequest);
    }
  }
}
