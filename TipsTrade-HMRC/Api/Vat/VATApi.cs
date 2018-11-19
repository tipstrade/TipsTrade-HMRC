using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using TipsTrade.HMRC.Api.Vat.Model;

namespace TipsTrade.HMRC.Api.Vat {
  /// <summary>The API that exposes VAT functions.</summary>
  public class VatApi : IApi, IClient {
    #region Properties
    /// <summary>The client used to make requests.</summary>
    Client IClient.Client { get; set; }

    /// <summary>The description of the API.</summary>
    public string Description => "An API for providing VAT data.";

    /// <summary>A flag indicating whether this version of the API is stable.</summary>
    public bool IsStable => false;

    /// <summary>The relative location of the API.</summary>
    public string Location => "organisations/vat";

    /// <summary>The name of the API.</summary>
    public string Name => "VAT (MTD) API";

    /// <summary>The version of the API that the client should target.</summary>
    public string Version => "1.0";
    #endregion

    #region Methods
    /// <summary>Retrieve VAT obligations.</summary>
    /// <param name="request">The obligations request.</param>
    public ObligationResult[] GetObligations(ObligationsRequest request) {
      var restRequest = this.CreateRequest($"{request.Vrn}/obligations", RestSharp.Method.GET, authorization: Authorization.User);
      restRequest.IsJsonContent();

      restRequest.AddDateRangeParameters(request);
      if (request.Status != null) {
        restRequest.AddParameter("status", $"{request.Status}"[0]);
      }

      var resp = this.ExecuteRequestList<ObligationResult>(restRequest, "obligations");

      // HACK: The Api appears to return all obligations, regardless of status, filter them here
      if (request.Status != null) {
        resp = resp.Where(x => x.Status == request.Status);
      }

      return resp.ToArray();
    }
    #endregion
  }
}
