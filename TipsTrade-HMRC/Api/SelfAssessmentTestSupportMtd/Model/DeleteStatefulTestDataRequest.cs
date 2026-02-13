using RestSharp;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model {
  /// <summary>The parameters used to delete stateful test data supplied by them in the sandbox environment.</summary>
  public class DeleteStatefulTestDataRequest : IApiRequest {
    #region Properties
    /// <summary>Optional National Insurance number, in the format AA999999A.</summary>
    /// <remarks>If supplied, the endpoint deletes only stateful test data stored against the vendor that is associated with the specified National Insurance number.</remarks>
    public string NiNumber { get; set; }
    #endregion

    #region Implementations
    /// <inheritdoc/>
    string IApiRequest.AcceptType => "json";

    /// <inheritdoc/>
    Authorization IApiRequest.Authorization => Authorization.User;

    /// <inheritdoc/>
    string IApiRequest.ContentType => "application/json";

    /// <inheritdoc/>
    Method IApiRequest.Method => Method.Delete;

    /// <inheritdoc/>
    string IApiRequest.Location => "vendor-state";

    /// <inheritdoc/>
    public void PopulateRequest(RestRequest request) {
      if (!string.IsNullOrEmpty(NiNumber)) {
        request.AddQueryParameter("nino", NiNumber);
      }
    }
    #endregion
  }
}
