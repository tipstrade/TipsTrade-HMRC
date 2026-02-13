using Newtonsoft.Json;
using System;
using System.Globalization;

namespace TipsTrade.HMRC.Api.Model.Converters {
  /// <summary>
  /// A custom JSON converter for serializing and deserializing <see cref="Nullable{DateTime}"/> objects
  /// in the "yyyy-MM-dd" format.
  /// </summary>
  public class NewtonsoftDateOnlyConverter : JsonConverter<DateTime?> {
    private const string DateFormat = "yyyy-MM-dd";

    /// <inheritdoc/>
    public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer) {
      if (reader.TokenType == JsonToken.Null) {
        if (Nullable.GetUnderlyingType(objectType) == null) {
          return default(DateTime);
        } else {
          return null;
        }
      } else if (reader.TokenType != JsonToken.String) {
        throw new JsonSerializationException($"Unexpected token parsing date. Expected String, got {reader.TokenType}.");
      }

      var dateString = reader.Value.ToString();

      if (DateTime.TryParseExact(dateString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed)) {
        return parsed;
      }

      throw new JsonSerializationException($"Invalid date format: {dateString}. Expected format is {DateFormat}.");
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer) {
      writer.WriteValue(value?.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
  }
}
