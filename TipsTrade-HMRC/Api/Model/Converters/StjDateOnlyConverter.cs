using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.Model.Converters {
  /// <summary>
  /// A custom JSON converter for serializing and deserializing <see cref="DateTime"/> objects
  /// in the "yyyy-MM-dd" format.
  /// </summary>
  public class StjDateOnlyConverter : JsonConverter<DateTime> {
    private const string DateFormat = "yyyy-MM-dd";

    /// <inheritdoc/>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
      if (reader.TokenType == JsonTokenType.Null) {
        if (Nullable.GetUnderlyingType(typeToConvert) == null) {
          return default(DateTime);
        } else {
          return default;
        }
      } else if (reader.TokenType != JsonTokenType.String) {
        throw new JsonException($"Unexpected token parsing date. Expected String, got {reader.TokenType}.");
      }

      var dateString = reader.GetString();

      if (DateTime.TryParseExact(dateString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed)) {
        return parsed;
      }

      throw new JsonException($"Invalid date format: {dateString}. Expected format is {DateFormat}.");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) {
      writer.WriteStringValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
  }
}
