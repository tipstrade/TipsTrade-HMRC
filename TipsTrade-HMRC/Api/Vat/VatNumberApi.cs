using System;
using TipsTrade.HMRC.Api.Vat.Model;

namespace TipsTrade.HMRC.Api.Vat {
  /// <summary>The API that exposes VAT number functions.</summary>
  public class VatNumberApi : IApi, IClient {
    /// <summary>The description of the API.</summary>
    public string Description => "Check a UK VAT number API.";

    /// <summary>A flag indicating whether this version of the API is stable.</summary>
    public bool IsStable => true;

    /// <summary>The relative location of the API.</summary>
    public string Location => "organisations/vat/check-vat-number";

    /// <summary>The name of the API.</summary>
    public string Name => "Check VAT Number API";

    /// <summary>The version of the API that the client should target.</summary>
    public string Version => "2.0";

    /// <summary>The client used to make requests.</summary>
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
