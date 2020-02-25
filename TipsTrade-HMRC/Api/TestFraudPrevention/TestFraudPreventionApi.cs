using TipsTrade.HMRC.AntiFraud;
using TipsTrade.HMRC.Api.TestFraudPrevention.Model;

namespace TipsTrade.HMRC.Api.TestFraudPrevention {
  /// <summary>The API that exposes Hello World function.</summary>
  public class TestFraudPreventionApi : IApi, IClient, IRequiresAntiFraud {
    #region Properties
    /// <summary>The client used to make requests.</summary>
    public Client Client { get; set; }

    /// <summary>The description of the API.</summary>
    public string Description => "An API for testing Fraud Prevention headers.";

    /// <summary>A flag indicating whether this version of the API is stable.</summary>
    public bool IsStable => false;

    /// <summary>The relative location of the API.</summary>
    public string Location => "test/fraud-prevention-headers";

    /// <summary>The name of the API.</summary>
    public string Name => "Test Fraud Prevention Headers API";

    /// <summary>The relative location of the API.</summary>
    public string Version => "1.0";
    #endregion

    #region Methods
    /// <summary>Validates fraud prevention headers submitted with this HTTP request.</summary>
    public ValidateResult Validate() {
      var restRequest = this.CreateRequest(new ValidateRequest());
      return this.ExecuteRequest<ValidateResult>(restRequest);
    }
    #endregion
  }
}
