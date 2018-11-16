using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Reflection;

namespace TipsTrade.HMRC.Json.Converters {
  /// <summary>Converts an <see cref="Enum"/> to its name value initial.</summary>
  public class CharEnumConverter : JsonConverter {
    /// <summary>Gets a value indicating whether this Newtonsoft.Json.JsonConverter can read JSON.</summary>
    public override bool CanRead => false;

    /// <summary>Gets a value indicating whether this Newtonsoft.Json.JsonConverter can write JSON.</summary>
    public override bool CanWrite => true;

    /// <summary>Determines whether this instance can convert the specified object type.</summary>
    public override bool CanConvert(Type objectType) {
      throw new NotImplementedException();
    }

    /// <summary>Reads the JSON representation of the object.</summary>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      throw new NotImplementedException();
    }

    /// <summary>Writes the JSON representation of the object.</summary>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      if (value == null) {
        writer.WriteNull();
        return;
      }

      var type = value.GetType();
      var name = Enum.GetName(type, value);
      var field = type.GetField(name);
      writer.WriteValue((field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? name)[0]);
    }
  }
}
