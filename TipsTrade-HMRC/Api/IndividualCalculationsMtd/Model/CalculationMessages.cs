using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model {
  /// <summary>Represents the messages associated with the retrieved tax calculation.</summary>
  public class CalculationMessages {
    /// <summary>A list of 'info' messages relating to the Calculation ID.</summary>
    [JsonProperty("info"), JsonPropertyName("info")]
    public IEnumerable<Message> Info { get; set; }

    /// <summary>A list of 'warning' messages relating to the Calculation ID.</summary>
    [JsonProperty("warnings"), JsonPropertyName("warnings")]
    public IEnumerable<Message> Warnings { get; set; }

    /// <summary>A list of 'error' messages relating to the Calculation ID.</summary>
    [JsonProperty("errors"), JsonPropertyName("errors")]
    public IEnumerable<Message> Errors { get; set; }

    /// <summary>Represents a message relating to the Calculation ID.</summary>
    public class Message {
      /// <summary>The ID of the message.</summary>
      [JsonProperty("id"), JsonPropertyName("id")]
      public string Id { get; set; }

      /// <summary>The message text.</summary>
      [JsonProperty("text"), JsonPropertyName("text")]
      public string Text { get; set; }
    }
  }
}
