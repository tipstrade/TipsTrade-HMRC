using Newtonsoft.Json;
using System;
using System.Globalization;

namespace TipsTrade.HMRC.Api.Model.Converters {
  /// <summary>
  /// A custom JSON converter for serializing and deserializing <see cref="DateTime"/> objects
  /// in the "yyyy-MM-dd" format.
  /// </summary>
  public class NewtonsoftDateOnlyConverter : JsonConverter<DateTime> {
    /// <inheritdoc/>
    public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer) {
      var dateString = reader.Value.ToString();

      if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed)) {
        return parsed;
      }

      throw new JsonSerializationException($"Invalid date format: {dateString}. Expected format is yyyy-MM-dd.");
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer) {
      writer.WriteValue(value.ToString("yyyy-MM-dd"));
    }
  }
}
