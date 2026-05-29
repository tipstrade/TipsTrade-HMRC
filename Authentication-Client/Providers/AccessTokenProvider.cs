using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.Tests.Authentication_Client.Providers {
  internal class AccessTokenProvider : IHmrcAccessTokenProvider {
    public Task<TokenResponse> GetCredentialAsync(string key, CancellationToken cancellationToken = default) {
      return Task.FromResult<TokenResponse>(null);
    }

    public Task SetCredentialAsync(string key, TokenResponse credential, CancellationToken cancellationToken = default) {
      return Task.CompletedTask;
    }
  }
}
