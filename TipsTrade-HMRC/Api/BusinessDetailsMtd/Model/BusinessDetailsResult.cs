using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TipsTrade.HMRC.Api.Model.Converters;

namespace TipsTrade.HMRC.Api.BusinessDetailsMtd.Model {
  /// <summary>Represents a result containing business details.</summary>
  public class BusinessDetailsResult : BusinessDetailsSummaryResult {
    #region Properties
    /// <summary>Year of migration.</summary>
    [JsonProperty("yearOfMigration"), JsonPropertyName("yearOfMigration")]
    public string YearOfMigration { get; set; }

    /// <summary>The first accounting period start date.</summary>
    /// <remarks>Accounting period start and end dates should not be displayed to users of your software.</remarks>
    [JsonProperty("firstAccountingPeriodStartDate"), JsonPropertyName("firstAccountingPeriodStartDate")]
    [Newtonsoft.Json.JsonConverter(typeof(NewtonsoftDateOnlyConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(StjDateOnlyConverter))]
    public DateTime? FirstAccountingPeriodStartDate { get; set; }

    /// <summary>The first accounting period end date.</summary>
    /// <remarks>Accounting period start and end dates should not be displayed to users of your software.</remarks>
    [JsonProperty("firstAccountingPeriodEndDate"), JsonPropertyName("firstAccountingPeriodEndDate")]
    [Newtonsoft.Json.JsonConverter(typeof(NewtonsoftDateOnlyConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(StjDateOnlyConverter))]
    public DateTime? FirstAccountingPeriodEndDate { get; set; }

    /// <summary>Income source latency details.</summary>
    [JsonProperty("latencyDetails"), JsonPropertyName("latencyDetails")]
    public Latency LatencyDetails { get; set; }

    /// <summary>Quarterly reporting period type.</summary>
    [JsonProperty("quarterlyTypeChoice"), JsonPropertyName("quarterlyTypeChoice")]
    public QuarterlyType QuarterlyTypeChoice { get; set; }

    /// <summary>The start and end dates of the latest accounting period for the business. This array contains dates for the latest accounting period only.</summary>
    /// <remarks>Accounting period start and end dates should not be displayed to users of your software.</remarks>
    [JsonProperty("accountingPeriods"), JsonPropertyName("accountingPeriods")]
    public IEnumerable<AccountingPeriod> AccountingPeriods { get; set; }

    /// <summary>Business start date, must be in the past.</summary>
    [JsonProperty("commencementDate"), JsonPropertyName("commencementDate")]
    [Newtonsoft.Json.JsonConverter(typeof(NewtonsoftDateOnlyConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(StjDateOnlyConverter))]
    public DateTime? CommencementDate { get; set; }

    /// <summary>Business cessation date.</summary>
    [JsonProperty("cessationDate"), JsonPropertyName("cessationDate")]
    [Newtonsoft.Json.JsonConverter(typeof(NewtonsoftDateOnlyConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(StjDateOnlyConverter))]
    public DateTime? CessationDate { get; set; }

    /// <summary>First line of the business address.</summary>
    [JsonProperty("businessAddressLineOne"), JsonPropertyName("businessAddressLineOne")]
    public string BusinessAddressLineOne { get; set; }

    /// <summary>Second line of the business address.</summary>
    [JsonProperty("businessAddressLineTwo"), JsonPropertyName("businessAddressLineTwo")]
    public string BusinessAddressLineTwo { get; set; }

    /// <summary>Third line of the business address.</summary>
    [JsonProperty("businessAddressLineThree"), JsonPropertyName("businessAddressLineThree")]
    public string BusinessAddressLineThree { get; set; }

    /// <summary>Fourth line of the business address.</summary>
    [JsonProperty("businessAddressLineFour"), JsonPropertyName("businessAddressLineFour")]
    public string BusinessAddressLineFour { get; set; }

    /// <summary>Business code.</summary>
    [JsonProperty("businessAddressPostcode"), JsonPropertyName("businessAddressPostcode")]
    public string BusinessAddressPostcode { get; set; }

    /// <summary>ISO country code for the business if it is in a non-GB country. The two-letter code for a country must adhere to ISO 3166.</summary>
    [JsonProperty("businessAddressCountryCode"), JsonPropertyName("businessAddressCountryCode")]
    public string BusinessAddressCountryCode { get; set; }
    #endregion

    #region Inner classes
    /// <summary>Represents accountancy period dates.</summary>
    public class AccountingPeriod {
      /// <summary>Date your books or accounts are made up to - the end of your accounting period.</summary>
      [JsonProperty("end"), JsonPropertyName("end")]
      [Newtonsoft.Json.JsonConverter(typeof(NewtonsoftDateOnlyConverter))]
      [System.Text.Json.Serialization.JsonConverter(typeof(StjDateOnlyConverter))]
      public DateTime End { get; set; }

      /// <summary>Date your books or accounts start - the beginning of your accounting period.</summary>
      [JsonProperty("start"), JsonPropertyName("start")]
      [Newtonsoft.Json.JsonConverter(typeof(NewtonsoftDateOnlyConverter))]
      [System.Text.Json.Serialization.JsonConverter(typeof(StjDateOnlyConverter))]
      public DateTime Start { get; set; }
    }

    /// <summary>Represents income source latency details.</summary>
    public class Latency {
      /// <summary>Indicator for annual self-assessment submission frequency.</summary>
      public const string IndicatorAnnually = "A";

      /// <summary>Indicator for quarterly self-assessment submission frequency.</summary>
      public const string IndicatorQuarterly = "Q";

      /// <summary>The end date of the latency period.</summary>
      [JsonProperty("latencyEndDate"), JsonPropertyName("latencyEndDate")]
      [Newtonsoft.Json.JsonConverter(typeof(NewtonsoftDateOnlyConverter))]
      [System.Text.Json.Serialization.JsonConverter(typeof(StjDateOnlyConverter))]
      public DateTime LatencyEndDate { get; set; }

      /// <summary>First tax year for the income source.</summary>
      [JsonProperty("taxYear1"), JsonPropertyName("taxYear1")]
      public string TaxYear1 { get; set; }

      /// <summary>Self-assessment submission frequency for the first tax year.</summary>
      /// <remarks>Possible values: "A" (Annually), "Q" (Quarterly).</remarks>
      [JsonProperty("latencyIndicator1"), JsonPropertyName("latencyIndicator1")]
      public string LatencyIndicator1 { get; set; }

      /// <summary>Second tax year for the income source.</summary>
      [JsonProperty("taxYear2"), JsonPropertyName("taxYear2")]
      public string TaxYear2 { get; set; }

      /// <summary>Self-assessment submission frequency for the second tax year.</summary>
      /// <remarks>Possible values: "A" (Annually), "Q" (Quarterly).</remarks>
      [JsonProperty("latencyIndicator2"), JsonPropertyName("latencyIndicator2")]
      public string LatencyIndicator2 { get; set; }
    }

    /// <summary>Represents Quarterly reporting period type.</summary>
    public class QuarterlyType {
      /// <summary>The quarterly period type that is being chosen for the business ID.</summary>
      /// <remarks>Possible values: "standard", "calendar".</remarks>
      [JsonProperty("quarterlyPeriodType"), JsonPropertyName("quarterlyPeriodType")]
      public string QuarterlyPeriodType { get; set; }

      /// <summary>The tax year for which the quarterly period type was chosen.</summary>
      /// <remarks>^2[0-9]{3}-[0-9]{2}$</remarks>
      [JsonProperty("taxYearOfChoice"), JsonPropertyName("taxYearOfChoice")]
      public string TaxYearOfChoice { get; set; }
    }
    #endregion
  }
}
