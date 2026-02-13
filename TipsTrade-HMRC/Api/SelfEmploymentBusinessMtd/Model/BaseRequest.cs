using RestSharp;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd.Model {
  /// <summary>The base parameters used to interact with the Self Employment Business (Mtd) endpoints.</summary>
  public abstract class BaseRequest : IApiRequest, IGovTestScenario {
    #region Properties
    /// <summary>A unique identifier for the business income source.</summary>
    /// <remarks>It must conform to the following regex: ^X[A-Z0-9]{1}IS[0-9]{11}$</remarks>
    public string BusinessId { get; set; }

    /// <summary>National Insurance number, in the format AA999999A.</summary>
    public string NiNumber { get; set; }

    /// <summary>The tax year to which the data applies. The start year and end year must not span two tax years. The minimum tax year is 2025-26. No gaps are allowed, for example, 2025-27 is not valid.</summary>
    /// <remarks>Example: 2025-26</remarks>
    public string TaxYear { get; set; }
    #endregion

    #region Interface Implementation
    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    /// <inheritdoc>
    public abstract Method Method { get; }

    /// <inheritdoc>
    public abstract string Location { get; }

    /// <inheritdoc/>
    public string GovTestScenario { get; set; }

    /// <inheritdoc/>
    public abstract void PopulateRequest(RestRequest request);
    #endregion
  }
}
