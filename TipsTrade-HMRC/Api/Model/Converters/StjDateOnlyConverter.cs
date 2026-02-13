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
    /// <inheritdoc/>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
      var dateString = reader.GetString();
   
      if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed)) {
        return parsed;
      }

      throw new JsonException($"Invalid date format: {dateString}. Expected format is yyyy-MM-dd.");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) {
      writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }
  }
}
