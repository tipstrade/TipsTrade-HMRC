using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.Vat.Model;

namespace TipsTrade.HMRC.Api.Vat {
  /// <summary>
  /// Service that exposes VAT number check functions, supporting dependency injection.
  /// </summary>
  public class VatNumberService : HmrcServiceBase {
    /// <inheritdoc/>
    public override string Description => "Check a UK VAT number API.";

    /// <inheritdoc/>
    public override bool IsStable => true;

    /// <inheritdoc/>
    public override string Location => "organisations/vat/check-vat-number";

    /// <inheritdoc/>
    public override string Name => "Check VAT Number API";

    /// <inheritdoc/>
    public override string Version => "2.0";

    /// <summary>Initialises a new instance using dependency-injected options.</summary>
    public VatNumberService(IOptions<HmrcOptions> options) : base(options) { }

    /// <summary>Initialises a new instance using a plain <see cref="HmrcOptions"/> object.</summary>
    public VatNumberService(HmrcOptions options) : base(options) { }

    /// <summary>Verifies the specified VAT registration number (VRN).</summary>
    public VatNumberCheckResponse CheckVrn(string vrn) {
      vrn = ValidateVrnOrThrow(vrn, nameof(vrn));
      return this.ExecuteRequest<VatNumberCheckResponse>(new VatNumberCheckRequest { Vrn = vrn });
    }

    /// <summary>Asynchronously verifies the specified VAT registration number (VRN).</summary>
    public async Task<VatNumberCheckResponse> CheckVrnAsync(string vrn, CancellationToken cancellationToken = default) {
      vrn = ValidateVrnOrThrow(vrn, nameof(vrn));
      return await this.ExecuteRequestAsync<VatNumberCheckResponse>(
        new VatNumberCheckRequest { Vrn = vrn },
        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Verifies the specified VAT registration number via a verified request made on behalf of a requester.</summary>
    public VerifiedVatNumberCheckResponse CheckVrn(string vrn, string requesterVrn) {
      vrn = ValidateVrnOrThrow(vrn, nameof(vrn));
      requesterVrn = ValidateVrnOrThrow(requesterVrn, nameof(requesterVrn));
      return this.ExecuteRequest<VerifiedVatNumberCheckResponse>(new VerifiedVatNumberCheckRequest { Vrn = vrn, RequesterVrn = requesterVrn });
    }

    /// <summary>Asynchronously verifies the specified VAT registration number via a verified request made on behalf of a requester.</summary>
    public async Task<VerifiedVatNumberCheckResponse> CheckVrnAsync(string vrn, string requesterVrn, CancellationToken cancellationToken = default) {
      vrn = ValidateVrnOrThrow(vrn, nameof(vrn));
      requesterVrn = ValidateVrnOrThrow(requesterVrn, nameof(requesterVrn));
      return await this.ExecuteRequestAsync<VerifiedVatNumberCheckResponse>(
        new VerifiedVatNumberCheckRequest { Vrn = vrn, RequesterVrn = requesterVrn },
        cancellationToken).ConfigureAwait(false);
    }

    private static string ValidateVrnOrThrow(string vrn, string originalParamName) {
      if (vrn == null) {
        throw new ArgumentException("VAT number cannot be null.", originalParamName);
      }

      vrn = vrn.Replace(" ", "");

      if (vrn.StartsWith("GB", StringComparison.OrdinalIgnoreCase)) {
        vrn = vrn.Substring(2);
      }

      if (vrn == "") {
        throw new ArgumentException("VAT number cannot be empty.", originalParamName);
      }

      return vrn;
    }
  }
}
