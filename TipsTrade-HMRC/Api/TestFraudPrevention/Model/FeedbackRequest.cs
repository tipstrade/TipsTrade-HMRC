using RestSharp;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.FraudPrevention;

namespace TipsTrade.HMRC.Api.TestFraudPrevention.Model {
  internal class FeedbackRequest : IApiRequestWithParameters {
    #region Properties
    /// <summary>
    /// The API endpoint to which the feedback is being submitted.
    /// </summary>
    /// <remarks>
    /// For the allowed values of this property, see <see href="https://developer.service.hmrc.gov.uk/api-documentation/docs/api/service/txm-fph-validator-api/1.0/oas/page#operation/GetfeedbackonrequestsmadetoanAPI"/>
    /// </remarks>
    public string Api { get; set; } = "";

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
    Method IApiRequest.Method => Method.Get;

    /// <inheritdoc/>
    string IApiRequest.Location => $"{Api}/validation-feedback";

    /// <inheritdoc/>
    void IApiRequestWithParameters.PopulateRequestParameters(RestRequest request) {
      request.AddQueryParameter("connectionMethod", ConnectionMethod.ToString());
    }
    #endregion
  }
}
