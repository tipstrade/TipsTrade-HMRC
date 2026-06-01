using System;
using System.Collections.Concurrent;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Api {
  /// <summary>A singleton cache that stores application-level <see cref="TokenResponse"/> instances, keyed by client ID.</summary>
  public class ApplicationTokenCache {
    private readonly ConcurrentDictionary<string, TokenResponse> _cache = new ConcurrentDictionary<string, TokenResponse>();

    /// <summary>
    /// Gets the cached <see cref="TokenResponse"/> for the specified <paramref name="clientId"/>,
    /// or <c>null</c> if no valid (non-expired) token is cached.
    /// </summary>
    public TokenResponse? Get(string clientId) {
      if (_cache.TryGetValue(clientId, out var token) && !token.HasAccessTokenExpired()) {
        return token;
      }

      _cache.TryRemove(clientId, out _); // Remove expired token if present

      return null;
    }

    /// <summary>Stores or replaces the <see cref="TokenResponse"/> for the specified <paramref name="clientId"/>.</summary>
    public void Set(string clientId, TokenResponse token) {
      if (clientId == null) {
        throw new ArgumentNullException(nameof(clientId));
      } else if (token == null) {
        throw new ArgumentNullException(nameof(token));
      }

      _cache[clientId] = token;
    }
  }
}
