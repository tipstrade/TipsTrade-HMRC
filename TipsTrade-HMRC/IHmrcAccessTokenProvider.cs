using TipsTrade.ApiClient.Core.Credential;
using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC {
  /// <summary>
  /// Provides methods for retrieving and setting HMRC API access tokens.
  /// </summary>
  public interface IHmrcAccessTokenProvider : IGetCredential<string, TokenResponse?>, ISetCredential<string, TokenResponse> {
  }
}
