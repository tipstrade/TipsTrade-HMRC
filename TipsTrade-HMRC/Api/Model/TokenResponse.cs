using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a response containing security tokens.</summary>
  public class TokenResponse {
    /// <summary>Gets the default number of minutes leeway that is used for determining if the <see cref="AccessToken"/> has expired.</summary>
    public const int DefaultSlewMinutes = 10;

    /// <summary>The access token.</summary>
    [JsonProperty("access_token"), JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = "";

    /// <summary>
    /// The UTC date at which the access token was created, defaulting to the current UTC time when the <see cref="TokenResponse"/> instance is created.
    /// </summary>
    /// <remarks>
    /// The OAuth server doesn't provide the creation timestamp, so it is set to the current UTC time by default. It can be used
    /// to calculate the expiration timestamp of the access token by adding the value of <see cref="ExpiresIn"/> to it.
    /// </remarks>
    [JsonProperty("x_created_at"), JsonPropertyName("x_created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>The number of seconds after which the access token will expire.</summary>
    [JsonProperty("expires_in"), JsonPropertyName("expires_in")]
    public double ExpiresIn { get; set; }

    /// <summary>The refresh token.</summary>
    [JsonProperty("refresh_token"), JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = "";

    /// <summary>The scope that the tokens apply to.</summary>
    [JsonProperty("scope"), JsonPropertyName("scope")]
    public string Scope { get; set; } = "";

    /// <summary>The type of token returned.</summary>
    [JsonProperty("token_type"), JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "";
  }

  /// <summary>
  /// Provides extension methods for the <see cref="TokenResponse"/> class.
  /// </summary>
  public static class TokenResponseExtensions {
    /// <summary>Gets the UTC date at which the <see cref="TokenResponse.AccessToken"/> will expire.</summary>
    /// <param name="tokenResponse">The token response to calculate the expiration timestamp for.</param> 
    /// <returns>The UTC date and time at which the access token will expire.</returns>
    public static DateTime GetExpiresTimestamp(this TokenResponse tokenResponse) {
      if (tokenResponse == null) {
        throw new ArgumentNullException(nameof(tokenResponse));
      }

      return tokenResponse.CreatedAt.AddSeconds(tokenResponse.ExpiresIn);
    }

    /// <summary>
    /// Determines whether the <see cref="TokenResponse.AccessToken"/> has expired, taking into account a leeway period defined by <paramref name="slewMinutes"/>.
    /// </summary>
    /// <param name="tokenResponse">The token response to check.</param>
    /// <param name="slewMinutes">The number of minutes leeway to use when comparing the expiration timestamp.</param>
    /// <returns>True if the access token has expired; otherwise, false.</returns>
    public static bool HasAccessTokenExpired(this TokenResponse tokenResponse, int slewMinutes = TokenResponse.DefaultSlewMinutes) {
      if (tokenResponse == null) {
        throw new ArgumentNullException(nameof(tokenResponse));
      }

      return DateTime.UtcNow >= tokenResponse.GetExpiresTimestamp().AddMinutes(-slewMinutes);
    }
  }
}