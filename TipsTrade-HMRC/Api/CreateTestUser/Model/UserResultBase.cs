using Newtonsoft.Json;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model {
  /// <summary>Represents the base object from which CreateUser responses must inherit.</summary>
  public abstract class UserResultBase {
    /// <summary>Government Gateway ID.</summary>
    [JsonProperty("userId")]
    public string UserId { get; set; }

    /// <summary>Government Gateway password.</summary>
    [JsonProperty("password")]
    public string Password { get; set; }

    /// <summary>Government Gateway user's full name.</summary>
    [JsonProperty("userFullName")]
    public string FullName { get; set; }

    /// <summary>Government Gateway user's email address.</summary>
    [JsonProperty("emailAddress")]
    public string Email { get; set; }
  }
}
