using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.Model;
using static TipsTrade.HMRC.Tests.TestBase;

namespace TipsTrade.HMRC.Tests.Providers {
  internal class AccessTokenProvider : IHmrcAccessTokenProvider {
    public Task<TokenResponse> GetCredentialAsync(string key, CancellationToken cancellationToken = default) {
      using (var fs = new FileStream("hmrc-users.json", FileMode.Open, FileAccess.Read)) {
        using (var reader = new StreamReader(fs)) {
          return Task.FromResult(JsonConvert.DeserializeObject<HmrcUsers>(reader.ReadToEnd()).Organisation.Tokens);
        }
      }
    }

    public Task SetCredentialAsync(string key, TokenResponse credential, CancellationToken cancellationToken = default) {
      return Task.CompletedTask;
    }
  }
}
