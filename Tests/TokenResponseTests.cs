using NUnit.Framework;
using System;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Tests {
  public class TokenResponseTests {
    [Test]
    public void Serialization_Deserialization_Should_Preserve_Properties() {
      var tokenResponse = new TokenResponse {
        AccessToken = "test_access_token",
        ExpiresIn = 3600,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        RefreshToken = "refresh_token",
        Scope = "test_scope",
        TokenType = "Bearer"
      };

      var json = System.Text.Json.JsonSerializer.Serialize(tokenResponse);
      var deserializedTokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(json);

      Assert.That(deserializedTokenResponse, Is.Not.Null);
      Assert.That(deserializedTokenResponse, Is.EqualTo(tokenResponse).UsingPropertiesComparer());
    }

    [Test]
    public void HasAccessTokenExpired_Should_Return_True_When_Expired() {
      var tokenResponse = new TokenResponse {
        AccessToken = "test_access_token",
        ExpiresIn = 3600,
        CreatedAt = DateTime.UtcNow.AddHours(-2), // Set created time in the past
        RefreshToken = "refresh_token",
        Scope = "test_scope",
        TokenType = "Bearer"
      };

      Assert.That(tokenResponse.HasAccessTokenExpired(), Is.True);
    }

    [Test]
    public void GetExpiresTimestamp_Throws_ArgumentNullException_When_TokenResponse_Is_Null() {
      TokenResponse? tokenResponse = null;
      var task = () => tokenResponse!.GetExpiresTimestamp();

      Assert.That(task, Throws.ArgumentNullException);
    }

    [Test]
    public void HasAccessTokenExpired_Throws_ArgumentNullException_When_TokenResponse_Is_Null() {
      TokenResponse? tokenResponse = null;
      var task = () => tokenResponse!.HasAccessTokenExpired();

      Assert.That(task, Throws.ArgumentNullException);
    }
  }
}