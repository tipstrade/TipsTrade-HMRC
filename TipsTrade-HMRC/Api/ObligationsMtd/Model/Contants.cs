namespace TipsTrade.HMRC.Api.ObligationsMtd.Model {
  /// <summary>Represents the possible statuses of an obligation.</summary>
  public static class ObligationStatus {
    /// <summary>Indicates that the obligation has been fulfilled.</summary>
    public const string Fulfilled = "fulfilled";

    /// <summary>Indicates that the obligation is still open.</summary>
    public const string Open = "open";
  }

  /// <summary>Represents the possible types of business.</summary>
  public static class TypeOfBusiness {
    /// <summary>Represents the type for self-employment business income source.</summary>
    public const string SelfEmployment = "self-employment";

    /// <summary>Represents the type for UK property business income source.</summary>
    public const string UkProperty = "uk-property";

    /// <summary>Represents the type for foreign property business income source.</summary>
    public const string ForeignProperty = "foreign-property";
  }
}
