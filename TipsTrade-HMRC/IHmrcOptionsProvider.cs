using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TipsTrade.HMRC {
  /// <summary>
  /// Defines a contract for providing HMRC options, which may include configuration settings, API endpoints, or other relevant information required for interacting with the HMRC API.
  /// </summary>
  public interface IHmrcOptionsProvider {
    /// <summary>
    /// Gets the HMRC options.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HMRC options.</returns>
    Task<HmrcOptions> GetOptionsAsync(CancellationToken cancellationToken = default);
  }

  /// <summary>
  /// A default implementation of the IHmrcOptionsProvider interface that retrieves HMRC options from an <see cref="IOptions{HmrcOptions}"/> instance, allowing for configuration via dependency injection.
  /// </summary>
  public class HmrcOptionsProvider : IHmrcOptionsProvider {
    private IOptions<HmrcOptions> Options { get; }

    /// <summary>
    /// Initializes a new instance of the HmrcOptionsProvider class.
    /// </summary>
    public HmrcOptionsProvider(IOptions<HmrcOptions> options) {
      Options = options ?? throw new ArgumentNullException(nameof(options), "HmrcOptions have not been configured.");
    }

    /// <inheritdoc/>
    public Task<HmrcOptions> GetOptionsAsync(CancellationToken cancellationToken = default) {
      return Task.FromResult(Options.Value);
    }
  }
}