using TipsTrade.HMRC.Api.Vat.Model;
using TipsTrade.HMRC.HelloWorld.Api.Model;

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
    /// <param name="obligations"></param>
    public Obligation[] GetObligations(ObligationsRequest obligations) {
      var request = this.CreateRequest($"{obligations.Vrn}/obligations", RestSharp.Method.GET, authorization: Authorization.User);
      request.AddHeader("Content-Type", "application/json");

      request.AddParameter("from", $"{obligations.From:yyyy-MM-dd}");
      request.AddParameter("to", $"{obligations.To:yyyy-MM-dd}");
      if (obligations.Status != null) {
        request.AddParameter("status", $"{obligations.Status}"[0]);
      }

      return this.ExecuteRequest<ObligationsResponse>(request).Obligations.ToArray();
    }
    #endregion
  }
}
