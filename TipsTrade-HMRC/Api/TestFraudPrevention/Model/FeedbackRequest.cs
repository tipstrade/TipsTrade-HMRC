using RestSharp;
using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.TestFraudPrevention.Model {
  internal class FeedbackRequest : IApiRequest {
    #region Properties
    /// <summary>
    /// The API endpoint to which the feedback is being submitted.
    /// </summary>
    /// <remarks>
    /// For the allowed values of this property, see <see href="https://developer.service.hmrc.gov.uk/api-documentation/docs/api/service/txm-fph-validator-api/1.0/oas/page#operation/GetfeedbackonrequestsmadetoanAPI"/>
    /// </remarks>
    public string Api { get; set; }

    /// <summary>
    /// The method by which the application connected to HMRC for the API request being submitted for feedback.
    /// </summary>
    public ConnectionMethod ConnectionMethod { get; set; }
    #endregion

    #region Implementations
    /// <inheritdoc/>
    string IApiRequest.AcceptType => "json";

    /// <inheritdoc/>
    Authorization IApiRequest.Authorization => Authorization.Application;

    /// <inheritdoc/>
    string IApiRequest.ContentType => null;

    /// <inheritdoc/>
    Method IApiRequest.Method => Method.Get;

    /// <inheritdoc/>
    string IApiRequest.Location => $"{Api}/validation-feedback";

    /// <inheritdoc/>
    void IApiRequest.PopulateRequest(RestRequest request) {
      request.AddQueryParameter("connectionMethod", ConnectionMethod.ToString());
    }
    #endregion
  }
}
