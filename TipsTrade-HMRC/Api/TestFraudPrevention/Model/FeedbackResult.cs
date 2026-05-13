using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.TestFraudPrevention.Model {
  /// <summary>
  /// Represents the result of feedback returned by the Test Fraud Prevention API.
  /// </summary>
  public class FeedbackResult {
    /// <summary>
    /// Gets or sets the collection of feedback requests.
    /// Each entry contains validation information for a single request.
    /// </summary>
    /// <value>An enumerable of <see cref="FeedbackEntry"/> instances, or <c>null</c> if none.</value>
    [JsonProperty("requests"), JsonPropertyName("requests")]
    public IEnumerable<FeedbackEntry> Requests { get; set; }
  }

  /// <summary>
  /// Represents feedback information for a single request.
  /// Includes headers validation and cross-validation results.
  /// </summary>
  public class FeedbackEntry {
    /// <summary>
    /// Gets or sets a code describing the request result.
    /// This is typically an identifier or status string returned by the API.
    /// </summary>
    [JsonProperty("code"), JsonPropertyName("code")]
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets cross-validation results for the request.
    /// Cross-validation entries validate relationships between headers or fields.
    /// </summary>
    [JsonProperty("crossValidation"), JsonPropertyName("crossValidation")]
    public IEnumerable<FeedbackEntryHeader> CrossValidation { get; set; }

    /// <summary>
    /// Gets or sets header validation results for the request.
    /// Each entry describes errors or warnings for a specific header.
    /// </summary>
    [JsonProperty("headers"), JsonPropertyName("headers")]
    public IEnumerable<FeedbackEntryHeader> Headers { get; set; }

    /// <summary>
    /// Gets or sets the HTTP method used for the request (e.g., "GET", "POST").
    /// </summary>
    [JsonProperty("method"), JsonPropertyName("method")]
    public string Method { get; set; }

    /// <summary>
    /// Gets or sets the request path that was validated.
    /// </summary>
    [JsonProperty("path"), JsonPropertyName("path")]
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the request was made.
    /// The timestamp is represented as a string in the format returned by the API.
    /// </summary>
    [JsonProperty("requestTimestamp"), JsonPropertyName("requestTimestamp")]
    public string RequestTimestamp { get; set; }

    /// <inheritdoc/>
    public override string ToString() {
      return $"{RequestTimestamp} {Method} {Path} {Code}";
    }
  }

  /// <summary>
  /// Represents validation details for a single header or validation rule.
  /// Contains errors, warnings and possibly nested header names.
  /// </summary>
  public class FeedbackEntryHeader {
    /// <summary>
    /// Gets or sets a code describing the header validation result.
    /// </summary>
    [JsonProperty("code"), JsonPropertyName("code")]
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the collection of errors for this header.
    /// Each string typically contains a human-readable error message or code.
    /// </summary>
    [JsonProperty("errors"), JsonPropertyName("errors")]
    public IEnumerable<string> Errors { get; set; }

    /// <summary>
    /// Gets or sets the name of the header being validated.
    /// </summary>
    [JsonProperty("header"), JsonPropertyName("header")]
    public string Header { get; set; }

    /// <summary>
    /// Gets or sets related header names when multiple headers are involved.
    /// </summary>
    [JsonProperty("headers"), JsonPropertyName("headers")]
    public IEnumerable<string> Headers { get; set; }

    /// <summary>
    /// Gets or sets the collection of warnings for this header.
    /// Warnings indicate non-fatal issues that should be reviewed.
    /// </summary>
    [JsonProperty("warnings"), JsonPropertyName("warnings")]
    public IEnumerable<string> Warnings { get; set; }

    /// <inheritdoc/>
    public override string ToString() {
      var headers = (Headers ?? Array.Empty<string>())
        .Union(new string[] { Header })
        .Where(x => !string.IsNullOrWhiteSpace(x));
      var hasErrors = this.HasErrors() ? "ERRORS" : "";
      var hasWarnings = this.HasWarnings() ? "WARNINGS" : "";

      return $"{Code} for [{string.Join(", ", headers)}] {hasErrors} {hasWarnings}".Trim();
    }
  }

  /// <summary>
  /// Extension methods to inspect feedback results for errors and warnings.
  /// </summary>
  public static class FeedbackResultExtensions {
    /// <summary>
    /// Determines whether any request in the feedback result contains errors.
    /// </summary>
    /// <param name="value">The <see cref="FeedbackResult"/> to inspect. May be <c>null</c>.</param>
    /// <returns><c>true</c> if any request has errors; otherwise <c>false</c>.</returns>
    public static bool HasErrors(this FeedbackResult value) => value?.Requests.Any(r => r.HasErrors()) == true;

    /// <summary>
    /// Determines whether the feedback entry contains errors either in cross-validation or headers.
    /// </summary>
    /// <param name="value">The <see cref="FeedbackEntry"/> to inspect. May be <c>null</c>.</param>
    /// <returns><c>true</c> if cross-validation or headers contain any errors; otherwise <c>false</c>.</returns>
    public static bool HasErrors(this FeedbackEntry value) {
        return value.CrossValidation?.Any(cv => cv.HasErrors()) == true
            || value.Headers?.Any(h => h.HasErrors()) == true;
    }

    /// <summary>
    /// Determines whether the feedback header contains any error messages.
    /// </summary>
    /// <param name="values">The <see cref="FeedbackEntryHeader"/> to inspect. May be <c>null</c>.</param>
    /// <returns><c>true</c> if the header contains one or more errors; otherwise <c>false</c>.</returns>
    public static bool HasErrors(this FeedbackEntryHeader values) => values?.Errors.Any() == true;

    /// <summary>
    /// Determines whether the feedback entry contains warnings either in cross-validation or headers.
    /// </summary>
    /// <param name="value">The <see cref="FeedbackEntry"/> to inspect. May be <c>null</c>.</param>
    /// <returns><c>true</c> if cross-validation or headers contain any warnings; otherwise <c>false</c>.</returns>
    public static bool HasWarnings(this FeedbackEntry value) {
        return value.CrossValidation?.Any(cv => cv.HasWarnings()) == true
            || value.Headers?.Any(h => h.HasWarnings()) == true;
    }

    /// <summary>
    /// Determines whether any request in the feedback result contains warnings.
    /// </summary>
    /// <param name="value">The <see cref="FeedbackResult"/> to inspect. May be <c>null</c>.</param>
    /// <returns><c>true</c> if any request has warnings; otherwise <c>false</c>.</returns>
    public static bool HasWarnings(this FeedbackResult value) => value?.Requests.Any(r => r.HasWarnings()) == true;

    /// <summary>
    /// Determines whether the feedback header contains any warning messages.
    /// </summary>
    /// <param name="values">The <see cref="FeedbackEntryHeader"/> to inspect. May be <c>null</c>.</param>
    /// <returns><c>true</c> if the header contains one or more warnings; otherwise <c>false</c>.</returns>
    public static bool HasWarnings(this FeedbackEntryHeader values) => values?.Warnings.Any() == true;
  }
}
