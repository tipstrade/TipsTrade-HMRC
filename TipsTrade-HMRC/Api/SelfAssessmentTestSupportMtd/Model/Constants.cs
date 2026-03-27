namespace TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd.Model {
  /// <summary>
  /// Represents the ITSA (Income Tax Self Assessment) statuses.
  /// </summary>
  public static class ItsaStatus {
    /// <summary>
    /// Indicates no status is assigned.
    /// </summary>
    public const string NoStatus = "No Status";

    /// <summary>
    /// Indicates the taxpayer is mandated to use MTD (Making Tax Digital).
    /// </summary>
    public const string MtdMandated = "MTD Mandated";

    /// <summary>
    /// Indicates the taxpayer has voluntarily opted into MTD.
    /// </summary>
    public const string MtdVoluntary = "MTD Voluntary";

    /// <summary>
    /// Indicates the taxpayer is on an annual reporting schedule.
    /// </summary>
    public const string Annual = "Annual";

    /// <summary>
    /// Indicates the taxpayer is not using digital reporting methods.
    /// </summary>
    public const string NonDigital = "Non Digital";

    /// <summary>
    /// Indicates the taxpayer's account is dormant.
    /// </summary>
    public const string Dormant = "Dormant";

    /// <summary>
    /// Indicates the taxpayer is exempt from MTD requirements.
    /// </summary>
    public const string MtdExempt = "MTD Exempt";
  }

  /// <summary>
  /// Represents the reasons for ITSA (Income Tax Self Assessment) statuses.
  /// </summary>
  public static class ItsaStatusReasons {
    /// <summary>
    /// Indicates a sign-up process where a return is available.
    /// </summary>
    public const string SignUpReturnAvailable = "Sign up - return available";

    /// <summary>
    /// Indicates a sign-up process where no return is available.
    /// </summary>
    public const string SignUpNoReturnAvailable = "Sign up - no return available";

    /// <summary>
    /// Indicates the final declaration for ITSA.
    /// </summary>
    public const string ItsFinalDeclaration = "ITSA final declaration";

    /// <summary>
    /// Indicates the Q4 declaration for ITSA.
    /// </summary>
    public const string ItsQ4Declaration = "ITSA Q4 declaration";

    /// <summary>
    /// Indicates a CESA (Centralized Electronic Self Assessment) SA return.
    /// </summary>
    public const string CesaSaReturn = "CESA SA return";

    /// <summary>
    /// Indicates a complex case.
    /// </summary>
    public const string Complex = "Complex";

    /// <summary>
    /// Indicates an income source has ceased.
    /// </summary>
    public const string CeasedIncomeSource = "Ceased income source";

    /// <summary>
    /// Indicates an income source has been reinstated.
    /// </summary>
    public const string ReinstatedIncomeSource = "Reinstated income source";

    /// <summary>
    /// Indicates a rollover process.
    /// </summary>
    public const string Rollover = "Rollover";

    /// <summary>
    /// Indicates changes in income source latency.
    /// </summary>
    public const string IncomeSourceLatencyChanges = "Income Source Latency Changes";

    /// <summary>
    /// Indicates the taxpayer has opted out of MTD ITSA.
    /// </summary>
    public const string MtdItsaOptOut = "MTD ITSA Opt-Out";

    /// <summary>
    /// Indicates the taxpayer has opted into MTD ITSA.
    /// </summary>
    public const string MtdItsaOptIn = "MTD ITSA Opt-In";

    /// <summary>
    /// Indicates the taxpayer is digitally exempt.
    /// </summary>
    public const string DigitallyExempt = "Digitally Exempt";
  }
}
