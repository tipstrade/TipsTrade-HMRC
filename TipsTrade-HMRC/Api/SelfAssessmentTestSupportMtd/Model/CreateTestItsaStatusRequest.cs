using RestSharp;
using System;
using System.Collections.Generic;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Extensions;

namespace TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model {
  /// <summary>The parameters used to create and amend a test ITSA status for a specified customer for use within the sandbox environment.</summary>
  public class CreateTestItsaStatusRequest : IApiRequest {
    #region Properties
    /// <summary>National Insurance number, in the format AA999999A.</summary>
    public string NiNumber { get; set; }

    /// <summary>The tax year to submit data for. The start year and end year must not span two tax years. No gaps are allowed - for example, 2023-25 is not valid. There is no minimum tax year.</summary>
    public string TaxYear { get; set; }

    /// <summary>Array of ITSA status details.</summary>
    public IEnumerable<ItsaStatusDetails> ItsaStatusDetails { get; set; }
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
    string IApiRequest.Location => $"itsa-status/{NiNumber}/{TaxYear}";

    /// <inheritdoc/>
    public void PopulateRequest(RestRequest request) {
      // Submitted on in the format YYYY-MM-DDThh:mm:ss.SSSZ
      var submittedOn = DateTime.Now.GetTaxYearStart().AddMonths(1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

      request.AddJsonBody(new {
        itsaStatusDetails = new dynamic[] {
          new {
            submittedOn = submittedOn,
            status = "MTD Mandated",
            statusReason = "Sign up - return available",
          }
        }
      });
    }
    #endregion
  }
}
