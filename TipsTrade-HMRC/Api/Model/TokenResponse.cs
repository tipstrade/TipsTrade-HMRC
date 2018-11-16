using Newtonsoft.Json;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a response containing security tokens.</summary>
  public class TokenResponse {
    /// <summary>The access token.</summary>
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    /// <summary>The number of seconds after which the </summary>
    [JsonProperty("expires_in")]
    public double Expires { get; set; }

    /// <summary>The refresh token.</summary>
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }

    /// <summary>The scope that the tokens apply to.</summary>
    [JsonProperty("scope")]
    public string Scope { get; set; }

    /// <summary>The type of token returned.</summary>
    [JsonProperty("token_type")]
    public string TokenType { get; set; }
  }
}
