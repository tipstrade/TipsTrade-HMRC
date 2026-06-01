using RestSharp;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api.ObligationsMtd.Model {
  /// <summary>The base parameters used to retrieve obligations.</summary>
  public abstract class BaseRequest : IApiRequest, IGovTestScenario {
    #region Properties
    /// <summary>National Insurance number, in the format AA999999A.</summary>
    public string NiNumber { get; set; }
    #endregion

    #region Interface Implementation
    /// <inheritdoc/>
    string IApiRequest.AcceptType => "json";

    /// <inheritdoc/>
    Authorization IApiRequest.Authorization => Authorization.User;

    /// <inheritdoc/>
    Method IApiRequest.Method => Method.Get;

    /// <inheritdoc/>
    public abstract string Location { get; }

    /// <inheritdoc/>
    public string GovTestScenario { get; set; }

    /// <inheritdoc/>
    public abstract void PopulateRequestParameters(RestRequest request);
    #endregion
  }
}
