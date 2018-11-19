namespace TipsTrade.HMRC.Api.Model {
  /// <summary>Represents a model that provides a Gov-Test-Scenario header.</summary>
  public interface IGovTestScenario {
    /// <summary>The Gov-Test-Scenario, only in the sandbox environment.</summary>
    string GovTestScenario { get; set; }
  }
}
