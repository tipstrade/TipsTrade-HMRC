using RestSharp;
using TipsTrade.HMRC.Api.Model;
using TipsTrade.HMRC.Api.Model.Attributes;

namespace TipsTrade.HMRC.Api.IndividualCalculationsMtd.Model {
  /// <summary>The parameters used to retrieve a Self Assessment tax calculation.</summary>
  public class RetrieveSelfAssessmentCalculationRequest : IApiRequest, IGovTestScenario {
    #region Gov-Test-Scenario constants
    /// <summary>Simulate a successful response.</summary>
    [GovTestScenario]
    public const string ScenarioDefault = "DEFAULT";

    /// <summary>Simulates the scenario where no data is found.</summary>
    [GovTestScenario]
    public const string ScenarioNotFound = "NOT_FOUND";

    /// <summary>Simulates the scenario where errors exist and no calculation has been generated.</summary>
    [GovTestScenario]
    public const string ScenarioErrorMessagesExist = "ERROR_MESSAGES_EXIST";

    /// <summary>Simulates an example tax calculation for a UK FHL Property business with Savings and Dividends.</summary>
    [GovTestScenario]
    public const string ScenarioUkMultipleIncomesExample = "UK_MULTIPLE_INCOMES_EXAMPLE";

    /// <summary>Simulates an example tax calculation for a UK Property (Non-FHL) business with Dividends.</summary>
    [GovTestScenario]
    public const string ScenarioUkPropDividendsExample = "UK_PROP_DIVIDENDS_EXAMPLE";

    /// <summary>Simulates an example tax calculation for a UK Property (Non-FHL) business with Gift Aid.</summary>
    [GovTestScenario]
    public const string ScenarioUkPropGiftAidExample = "UK_PROP_GIFTAID_EXAMPLE";

    /// <summary>Simulates an example tax calculation for a UK Property (Non-FHL) business with Savings.</summary>
    [GovTestScenario]
    public const string ScenarioUkPropSavingsExample = "UK_PROP_SAVINGS_EXAMPLE";

    /// <summary>Simulates an example tax calculation for a UK Self-Employment business with Gift Aid.</summary>
    [GovTestScenario]
    public const string ScenarioUkSeGiftAidExample = "UK_SE_GIFTAID_EXAMPLE";

    /// <summary>Simulates an example tax calculation for a UK Self-Employment business with Savings.</summary>
    [GovTestScenario]
    public const string ScenarioUkSeSavingsExample = "UK_SE_SAVINGS_EXAMPLE";

    /// <summary>Simulates an example tax calculation for a Scottish Self-Employment business with Dividends.</summary>
    [GovTestScenario]
    public const string ScenarioScotSeDividendsExample = "SCOT_SE_DIVIDENDS_EXAMPLE";

    /// <summary>The date fields in the response are made dynamic based on the tax year passed within the request.</summary>
    /// <remarks>
    /// The dynamic response will work for tax years (taxYear) starting 2023-24 onwards. 
    /// For any previous tax years supplied, it will result in a HTTP 400 (RULE_INCORRECT_GOV_TEST_SCENARIO) error being returned.
    /// </remarks>
    [GovTestScenario]
    public const string ScenarioDynamic = "DYNAMIC";

    #endregion

    #region Properties
    /// <summary>The National Insurance number of the individual.</summary>
    /// <remarks>Example: "AA123456A"</remarks>
    public string NiNumber { get; set; }

    /// <summary>The tax year for which the calculation is being retrieved.</summary>
    /// <remarks>Example: "2023-24"</remarks>
    public string TaxYear { get; set; }

    /// <summary>The unique calculation ID.</summary>
    /// <remarks>Example: "f2fb30e5-8e8f-4c4a-a8a4-ff978a7ee7e1"</remarks>
    public string CalculationId { get; set; }
    #endregion

    #region Impementations
    string IApiRequest.AcceptType => "json";

    Authorization IApiRequest.Authorization => Authorization.User;

    string IApiRequest.ContentType => "application/json";

    Method IApiRequest.Method => Method.Get;

    string IApiRequest.Location => $"{NiNumber}/self-assessment/{TaxYear}/{CalculationId}";

    /// <inheritdoc/>
    public string GovTestScenario { get; set; }

    void IApiRequest.PopulateRequest(RestRequest request) {
    }
    #endregion
  }
}