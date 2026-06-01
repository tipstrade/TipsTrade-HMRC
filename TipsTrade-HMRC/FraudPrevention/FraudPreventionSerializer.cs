using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using TipsTrade.HMRC.FraudPrevention.Headers;

namespace TipsTrade.HMRC.FraudPrevention {
  /// <summary>Provides serialization and validation of <see cref="IFraudPrevention"/> headers.</summary>
  public static class FraudPreventionSerializer {
    /// <summary>Returns a dictionary of HTTP header name/value pairs, throwing if validation fails.</summary>
    public static Dictionary<string, string> ToHttpHeaders(this IFraudPrevention source) {
      var errors = new List<string>();
      var headers = Serialize(source.GetHeaders(), errors);

      if (errors.Count > 0) {
        throw new FraudPreventionException($"{errors.Count} validation error(s) were found.") {
          Errors = errors
        };
      }

      return headers;
    }

    /// <summary>Returns a flag indicating whether all headers are valid.</summary>
    public static bool Validate(this IFraudPrevention source) => source.Validate(out _);

    /// <summary>Returns a flag indicating whether all headers are valid.</summary>
    /// <param name="source">The fraud prevention instance to validate.</param>
    /// <param name="errors">Populated with any validation errors.</param>
    public static bool Validate(this IFraudPrevention source, out string[] errors) {
      var list = new List<string>();
      Serialize(source.GetHeaders(), list);
      errors = list.ToArray();
      return list.Count == 0;
    }

    private static Dictionary<string, string> Serialize(IEnumerable<FraudPreventionHeader> headers, List<string> errors) {
      var result = new Dictionary<string, string>();

      foreach (var header in headers) {
        var value = header.Value;

        bool isEmpty;
        if (value == null) {
          isEmpty = true;
        } else if (value is IEnumerable list && value is not string) {
          isEmpty = !list.Any();
        } else if ("".Equals(value)) {
          isEmpty = true;
        } else {
          isEmpty = false;
        }

        if (isEmpty && !header.AllowEmpty) {
          errors.Add($"{header.Name} cannot be empty.");
          continue;
        }

        if (isEmpty) {
          continue;
        }

        string headerValue;

        if (value is string str) {
          headerValue = HttpUtility.UrlEncode(str);

        } else if (value is IDictionary dict) {
          var sb = new StringBuilder();
          foreach (var key in dict.Keys) {
            if (sb.Length != 0) sb.Append("&");
            sb.AppendFormat("{0}={1}", HttpUtility.UrlEncode($"{key}"), HttpUtility.UrlEncode($"{dict[key]}"));
          }
          headerValue = sb.ToString();

        } else if (value is IEnumerable enumerable) {
          var sb = new StringBuilder();
          foreach (var o in enumerable) {
            if (sb.Length != 0) sb.Append(",");
            if (o == null) {
              errors.Add($"{header.Name} contains a null value.");
              continue;
            } else if (o is IFraudPreventionValue val) {
              sb.Append(val.GetHeaderValue());
            } else {
              sb.Append(HttpUtility.UrlEncode($"{o}"));
            }
          }
          headerValue = sb.ToString();

        } else if (value is IFraudPreventionValue afVal) {
          headerValue = afVal.GetHeaderValue();

        } else if (value is TimeZoneInfo tz) {
          var symbol = tz.BaseUtcOffset.TotalHours >= 0 ? "+" : "-";
          headerValue = $"UTC{symbol}{tz.BaseUtcOffset:hh\\:mm}";

        } else if (value is DateTime date) {
          headerValue = $"{date.ToUniversalTime():yyyy-MM-dd'T'HH:mm:ss.fff'Z'}";

        } else {
          headerValue = HttpUtility.UrlEncode($"{value}");
        }

        result[header.Name] = headerValue;
      }

      return result;
    }
  }
}
