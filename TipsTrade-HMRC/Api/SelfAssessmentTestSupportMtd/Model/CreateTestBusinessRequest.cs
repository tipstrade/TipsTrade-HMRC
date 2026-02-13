using RestSharp;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model {
  /// <summary>The parameters used to create a test business for use within the Sandbox environment.</summary>
  public class CreateTestBusinessRequest : IApiRequest {
    #region Properties
    /// <summary>Optional National Insurance number, in the format AA999999A.</summary>
    /// <remarks>If supplied, the endpoint deletes only stateful test data stored against the vendor that is associated with the specified National Insurance number.</remarks>
    public string NiNumber { get; set; }

    /// <summary>The details of the business to create.</summary>
    public BusinessDetailsMtd.Model.BusinessDetailsResult BusinessDetails { get; set; }
    #endregion

    #region Implementations
    /// <inheritdoc/>
    string IApiRequest.AcceptType => "json";

    /// <inheritdoc/>
    Authorization IApiRequest.Authorization => Authorization.User;

    /// <inheritdoc/>
    string IApiRequest.ContentType => "application/json";

    /// <inheritdoc/>
    Method IApiRequest.Method => Method.Post;

    /// <inheritdoc/>
    string IApiRequest.Location => $"business/{NiNumber}";

    /// <inheritdoc/>
    public void PopulateRequest(RestRequest request) {
      request.AddJsonBody(BusinessDetails);
    }
    #endregion
  }
}
