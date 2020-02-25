namespace TipsTrade.HMRC.Api.TestFraudPrevention {
  /// <summary>The list of valid values for the fraud prevention validation codes.</summary>
  public enum ValidationCode {
    /// <summary>	Basic checks show that the header value supplied appears to be valid.</summary>
    VALID_HEADERS,
    /// <summary>Basic checks show that at least one of the header values supplied is not valid.</summary>
    INVALID_HEADERS,
    /// <summary>Basic checks show that the header value supplied is not valid.</summary>
    INVALID_HEADER,
    /// <summary>
    /// Basic checks show that at least one of the header values supplied is potentially invalid as per current specification.
    /// However, this might turn into an INVALID_HEADER when further checks are performed or fraud prevention header specification
    /// is updated.
    /// </summary>
    POTENTIALLY_INVALID_HEADERS,
    /// <summary>
    /// Basic checks show that the header value supplied is potentially invalid as per current specification.
    /// However, this might turn into INVALID_HEADER when further checks are performed or fraud prevention header
    /// specification is updated.
    /// </summary>
    POTENTIALLY_INVALID_HEADER,
    /// <summary>Header required for the connection method is not supplied.</summary>
    MISSING_HEADER,
    /// <summary>Header required for the connection method is empty.</summary>
    EMPTY_HEADER,
    /// <summary>Header value supplied is not a fraud prevention header, or the fraud prevention header is misspelt.</summary>
    UNEXPECTED_HEADER
  }
}
