using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model {
  /// <summary>The parameters used to create and amend a test ITSA status for a specified customer for use within the sandbox environment.</summary>
  public class CreateTestItsaStatusRequest : IApiRequestWithBody {
    #region Properties
    /// <summary>National Insurance number, in the format AA999999A.</summary>
    public string NiNumber { get; set; } = "";

    /// <summary>The tax year to submit data for. The start year and end year must not span two tax years. No gaps are allowed - for example, 2023-25 is not valid. There is no minimum tax year.</summary>
    public string TaxYear { get; set; } = "";

    /// <summary>Array of ITSA status details.</summary>
    public IEnumerable<ItsaStatusDetails>? ItsaStatusDetails { get; set; }
    #endregion

    #region Implementations
    /// <inheritdoc/>
    string IApiRequest.AcceptType => "json";

    /// <inheritdoc/>
    Authorization IApiRequest.Authorization => Authorization.User;

    /// <inheritdoc/>
    string IApiRequestWithBody.ContentType => "application/json";

    /// <inheritdoc/>
    Method IApiRequest.Method => Method.Post;

    /// <inheritdoc/>
    string IApiRequest.Location => $"itsa-status/{NiNumber}/{TaxYear}";

    /// <inheritdoc/>
    public void PopulateRequestParameters(RestRequest request) {
    }

    /// <inheritdoc/>
    void IApiRequestWithBody.PopulateRequestBody(RestRequest request) {
      if (ItsaStatusDetails == null) {
        throw new InvalidOperationException("ITSA status details must be provided.");
      }

      var details = ItsaStatusDetails.ToArray();

      if (details.Length == 0) {
        throw new InvalidOperationException("At least one ITSA status detail must be provided.");
      }
      
      request.AddJsonBody(new {
        itsaStatusDetails = details
      });
    }
    #endregion
  }
}
