using RestSharp;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>
  /// Represents a model that all Api requests must inherit from.
  /// </summary>
  internal interface IApiRequest {
    /// <summary>
    /// Gets the expected MIME type for responses returned by the API for this request.
    /// Typically a value such as "json" used to form "application/vnd.hmrc.{Version}+{AcceptType}" in the Accept header.
    /// </summary>
    string AcceptType { get; }

    /// <summary>
    /// Gets the authorization level required to execute this request.
    /// Possible values indicate whether the request is open, requires a user token, or an application token.
    /// </summary>
    Authorization Authorization { get; }

    /// <summary>
    /// Gets the HTTP method used by this request (for example: GET, POST, PUT, DELETE).
    /// </summary>
    Method Method { get; }

    /// <summary>
    /// Gets the relative resource location (path) for this request.
    /// This value is combined with the API base URL to form the full request URL.
    /// </summary>
    string Location { get; }

    /// <summary>
    /// Populates the provided <see cref="RestRequest"/> with any required query parameters or headers.
    /// Implementations should not add body content here; use <see cref="IApiRequestWithBody.PopulateRequestBody"/> for that.
    /// </summary>
    /// <param name="request">The <see cref="RestRequest"/> instance to populate.</param>
    void PopulateRequestParameters(RestRequest request);
  }

  internal interface IApiRequestWithBody : IApiRequest {
    /// <summary>
    /// Gets the MIME type used for the request body when sending data to the API.
    /// For example: "application/json" or "application/x-www-form-urlencoded".
    /// </summary>
    string ContentType { get; }

    /// <summary>
    /// Populates the provided <see cref="RestRequest"/> with the request body content.
    /// </summary>
    /// <param name="request">The <see cref="RestRequest"/> instance to populate.</param>
    void PopulateRequestBody(RestRequest request);
  }
}
