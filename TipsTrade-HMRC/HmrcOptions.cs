namespace TipsTrade.HMRC {
  /// <summary>Configuration options for the HMRC API services.</summary>
  public class HmrcOptions {
    /// <summary>The location of the production API.</summary>
    public const string ProductionUrl = "https://api.service.hmrc.gov.uk";

    /// <summary>The location of the sandbox API.</summary>
    public const string SandboxUrl = "https://test-api.service.hmrc.gov.uk";

    /// <summary>Configuration options for the fraud prevention service.</summary>
    public FraudPrevention.IFraudPrevention? FraudPreventionConfig { get; set; }

    /// <summary>Gets the base URL used for all requests, based on the current environment.</summary>
    public string BaseUrl => IsSandbox ? SandboxUrl : ProductionUrl;

    /// <summary>The ID used to identify your application during each step of an OAuth 2.0 journey.</summary>
    public string? ClientID { get; set; }

    /// <summary>The secret passphrase used to authorise your application during each step of an OAuth 2.0 journey.</summary>
    public string? ClientSecret { get; set; }

    /// <summary>A flag indicating whether the services are accessing the sandbox environment.</summary>
    public bool IsSandbox { get; set; } = false;
  }
}
