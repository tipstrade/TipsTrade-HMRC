using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>Represents the base object from which CreateUser responses must inherit.</summary>
  public abstract class UserResultBase {
    /// <summary>Government Gateway ID.</summary>
    [JsonProperty("userId"), JsonPropertyName("userId")]
    public string UserId { get; set; }

    /// <summary>Government Gateway password.</summary>
    [JsonProperty("password"), JsonPropertyName("password")]
    public string Password { get; set; }

    /// <summary>Government Gateway user's full name.</summary>
    [JsonProperty("userFullName"), JsonPropertyName("userFullName")]
    public string FullName { get; set; }

    /// <summary>Government Gateway user's email address.</summary>
    [JsonProperty("emailAddress"), JsonPropertyName("emailAddress")]
    public string Email { get; set; }

    /// <summary>Returns a string that represents the current object.</summary>
    public override string ToString() {
      return $"Email: {Email}, Password: {Password}";
    }
  }
}
