using System;
using System.Threading;
using System.Threading.Tasks;
using TipsTrade.HMRC.Api.Vat.Model;

namespace TipsTrade.HMRC.Api.Vat {
  /// <summary>
  /// The API that exposes VAT number functions.
  /// </summary>
  /// <remarks>
  /// Provides methods to check and verify UK VAT registration numbers (VRNs). Methods accept VRNs
  /// in a relaxed form and will normalize them by removing whitespace and a leading "GB"
  /// country prefix where present.
  /// </remarks>
  public class VatNumberApi : IApi, IClient {
    #region Properties
    /// <inheritdoc/>
    public string Description => "Check a UK VAT number API.";

    /// <inheritdoc/>
    public bool IsStable => true;

    /// <inheritdoc/>
    public string Location => "organisations/vat/check-vat-number";

    /// <inheritdoc/>
    public string Name => "Check VAT Number API";

    /// <inheritdoc/>
    public string Version => "2.0";

    /// <inheritdoc/>
    public Client Client { get; set; }
    #endregion

    #region API methods
    /// <summary>
    /// Verifies the specified VAT registration number (VRN).
    /// </summary>
    /// <param name="vrn">The VAT registration number to check. May include leading whitespace or the "GB" prefix.</param>
    /// <returns>
    /// A <see cref="VatNumberCheckResponse"/> containing details about the supplied VAT number.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the supplied <paramref name="vrn"/> is null or empty after parsing.</exception>
    public VatNumberCheckResponse CheckVrn(string vrn) {
      vrn = ValidateVrnOrThrow(vrn, nameof(vrn));

      return this.ExecuteRequest<VatNumberCheckResponse>(new VatNumberCheckRequest { Vrn = vrn });
    }

    /// <summary>
    /// Asynchronously verifies the specified VAT registration number (VRN).
    /// </summary>
    /// <param name="vrn">The VAT registration number to check. May include leading whitespace or the "GB" prefix.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is a <see cref="VatNumberCheckResponse"/>
    /// containing details about the supplied VAT number.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the supplied <paramref name="vrn"/> is null or empty after parsing.</exception>
    public async Task<VatNumberCheckResponse> CheckVrnAsync(string vrn, CancellationToken cancellationToken = default) {
      vrn = ValidateVrnOrThrow(vrn, nameof(vrn));

      return await this.ExecuteRequestAsync<VatNumberCheckResponse>(
        new VatNumberCheckRequest { Vrn = vrn },
        cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies the specified VAT registration number (VRN) via a verified request made on behalf of a requester.
    /// </summary>
    /// <param name="vrn">The VAT registration number to check. May include leading whitespace or the "GB" prefix.</param>
    /// <param name="requesterVrn">The VAT registration number of the requester performing the verified check.</param>
    /// <returns>
    /// A <see cref="VerifiedVatNumberCheckResponse"/> containing verified details about the supplied VAT number.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the supplied <paramref name="vrn"/> is null or empty after parsing.</exception>
    public VerifiedVatNumberCheckResponse CheckVrn(string vrn, string requesterVrn) {
      vrn = ValidateVrnOrThrow(vrn, nameof(vrn));
      requesterVrn = ValidateVrnOrThrow(requesterVrn, nameof(requesterVrn));

      return this.ExecuteRequest<VerifiedVatNumberCheckResponse>(new VerifiedVatNumberCheckRequest { Vrn = vrn, RequesterVrn = requesterVrn });
    }

    /// <summary>
    /// Asynchronously verifies the specified VAT registration number (VRN) via a verified request made on behalf of a requester.
    /// </summary>
    /// <param name="vrn">The VAT registration number to check. May include leading whitespace or the "GB" prefix.</param>
    /// <param name="requesterVrn">The VAT registration number of the requester performing the verified check.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is a <see cref="VerifiedVatNumberCheckResponse"/>
    /// containing verified details about the supplied VAT number.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the supplied <paramref name="vrn"/> is null or empty after parsing.</exception>
    public async Task<VerifiedVatNumberCheckResponse> CheckVrnAsync(string vrn, string requesterVrn, CancellationToken cancellationToken = default) {
      vrn = ValidateVrnOrThrow(vrn, nameof(vrn));
      requesterVrn = ValidateVrnOrThrow(requesterVrn, nameof(requesterVrn));

      return await this.ExecuteRequestAsync<VerifiedVatNumberCheckResponse>(
        new VerifiedVatNumberCheckRequest { Vrn = vrn, RequesterVrn = requesterVrn },
        cancellationToken
        ).ConfigureAwait(false);
    }
    #endregion

    #region Other methods
    /// <summary>
    /// Validates and normalizes a VAT registration number (VRN) or throws an <see cref="ArgumentException"/>.
    /// </summary>
    /// <remarks>
    /// The method accepts VRNs in a relaxed form. It removes all whitespace and strips a leading
    /// "GB" country prefix (case-insensitive) when present, returning the cleaned VRN.
    /// </remarks>
    /// <param name="vrn">
    /// The VAT registration number to validate. May be null, contain whitespace, or include a leading
    /// "GB" prefix. This value is not modified in the caller; the normalized VRN is returned.
    /// </param>
    /// <param name="originalParamName">
    /// The original parameter name to include in any thrown <see cref="ArgumentException"/> so that
    /// the exception message correctly identifies the caller's parameter.
    /// </param>
    /// <returns>
    /// The normalized VRN with whitespace removed and any leading "GB" prefix stripped.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="vrn"/> is <c>null</c> or empty after normalization.
    /// </exception>
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
    #endregion
  }
}
