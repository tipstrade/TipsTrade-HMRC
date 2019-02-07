using Newtonsoft.Json;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using System.IO;

namespace TipsTrade.HMRC.Serialization {
  /// <summary>Represents a serializer that usesthe <see cref="Newtonsoft.Json.JsonSerializer"/>.</summary>
  public class NewtonsoftJsonSerializer : ISerializer, IDeserializer {
    private Newtonsoft.Json.JsonSerializer serializer;

    /// <summary>Creates an instance of the <see cref="NewtonsoftJsonSerializer"/> class.</summary>
    public NewtonsoftJsonSerializer(Newtonsoft.Json.JsonSerializer serializer) {
      this.serializer = serializer;
    }

    /// <summary>Gets the content-type for JSON.</summary>
    public string ContentType {
      get { return "application/json"; } // Probably used for Serialization?
      set { }
    }

    /// <summary>Gets or sets the data format to be used.</summary>
    public string DateFormat { get; set; }

    /// <summary>Gets or sets the namespace to be used.</summary>
    public string Namespace { get; set; }

    /// <summary>Gets or sets the root element to be used.</summary>
    public string RootElement { get; set; }

    /// <summary>Returns a string containing the serialized value.</summary>
    public string Serialize(object obj) {
      using (var stringWriter = new StringWriter()) {
        using (var jsonTextWriter = new JsonTextWriter(stringWriter)) {
          serializer.Serialize(jsonTextWriter, obj);

          return stringWriter.ToString();
        }
      }
    }

    /// <summary>Returns the deserialized response of the specified type.</summary>
    /// <typeparam name="T">The type of object to be deserialized.</typeparam>
    /// <param name="response">The response containing the content to deserialize.</param>
    public T Deserialize<T>(RestSharp.IRestResponse response) {
      var content = response.Content;

      using (var stringReader = new StringReader(content)) {
        using (var jsonTextReader = new JsonTextReader(stringReader)) {
          return serializer.Deserialize<T>(jsonTextReader);
        }
      }
    }

    /// <summary>Gets the default instance of the <see cref="NewtonsoftJsonSerializer"/> class.</summary>
    public static NewtonsoftJsonSerializer Default {
      get {
        return new NewtonsoftJsonSerializer(new Newtonsoft.Json.JsonSerializer() {
          NullValueHandling = NullValueHandling.Ignore,
        });
      }
    }
  }
}
