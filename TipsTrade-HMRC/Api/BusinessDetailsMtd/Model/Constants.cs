namespace TipsTrade.HMRC.Api.BusinessDetailsMtd.Model {
  /// <summary>Represents the possible types of business.</summary>
  public static class TypeOfBusiness {
    /// <summary>Represents the type for self-employment business income source.</summary>
    public const string SelfEmployment = "self-employment";

    /// <summary>Represents the type for UK property business income source.</summary>
    public const string UkProperty = "uk-property";

    /// <summary>Represents the type for foreign property business income source.</summary>
    public const string ForeignProperty = "foreign-property";

    /// <summary>Represents the type for unspecified property business income source.</summary>
    public const string PropertyUnspecified = "property-unspecified";
  }

  /// <summary>Represents the possible quarter types.</summary>
  public static class  QuarterType  {
    /// <summary>Represents the standard quarterly period type.</summary>
    public const string Standard = "standard";

    /// <summary>Represents the calendar quarterly period type.</summary>
    public const string Calendar = "calendar";
  }
}
