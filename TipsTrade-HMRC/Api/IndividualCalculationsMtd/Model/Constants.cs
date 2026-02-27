namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model {
  /// <summary>
  /// Represents the outcomes of calculations in the Individual Calculations MTD API.
  /// </summary>
  public static class CalculationOutcome {
    /// <summary>
    /// Represents a processed calculation outcome.
    /// </summary>
    public const string Processed = "PROCESSED";

    /// <summary>
    /// Represents an error calculation outcome.
    /// </summary>
    public const string Error = "ERROR";

    /// <summary>
    /// Represents a rejected calculation outcome.
    /// </summary>
    public const string Rejected = "REJECTED";
  }

  /// <summary>
  /// Represents the triggers for calculations in the Individual Calculations MTD API.
  /// </summary>
  public static class CalculationTrigger {
    /// <summary>
    /// Represents an attended calculation trigger.
    /// </summary>
    public const string Attended = "attended";

    /// <summary>
    /// Represents a Class 2 National Insurance Contribution (NIC) event calculation trigger.
    /// </summary>
    public const string Class2NICEvent = "class2NICEvent";

    /// <summary>
    /// Represents an unattended calculation trigger.
    /// </summary>
    public const string Unattended = "unattended";

    /// <summary>
    /// Represents a calculation trigger for a CESA Self-Assessment (SA) return.
    /// </summary>
    public const string CesaSAReturn = "cesaSAReturn";

    /// <summary>
    /// Represents a calculation trigger for a new loss event.
    /// </summary>
    public const string NewLossEvent = "newLossEvent";

    /// <summary>
    /// Represents a calculation trigger for an updated loss event.
    /// </summary>
    public const string UpdatedLossEvent = "updatedLossEvent";
  }

  /// <summary>
  /// Represents the types of calculations available in the Individual Calculations MTD API.
  /// </summary>
  public static class CalculationType {
    /// <summary>
    /// Represents a calculation type for confirming an amendment to a prior submission.
    /// </summary>
    public const string ConfirmAmendment = "confirm-amendment";

    /// <summary>
    /// Represents a calculation type for making a final declaration.
    /// </summary>
    public const string FinalDeclaration = "final-declaration";

    /// <summary>
    /// Represents a calculation type for an in-year tax calculation.
    /// </summary>
    public const string InYear = "in-year";

    /// <summary>
    /// Represents a calculation type for indicating an intent to finalize a submission.
    /// </summary>
    public const string IntentToFinalise = "intent-to-finalise";

    /// <summary>
    /// Represents a calculation type for indicating an intent to amend a submission.
    /// </summary>
    public const string IntentToAmend = "intent-to-amend";
  }
}
