using System;
using System.Collections.Generic;

namespace TipsTrade.HMRC.Api.Model.Attributes {
  /// <summary>Represents an attribute that indicates that the field is a Gov Test Scenario.</summary>
  [AttributeUsage(AttributeTargets.Field)]
  public class GovTestScenarioAttribute : Attribute {
    /// <summary>Gets the collection of error codes expected.</summary>
    public IEnumerable<string> ErrorCodes { get; }

    /// <summary>Creates an instance of the <see cref="GovTestScenarioAttribute"/> class.</summary>
    /// <param name="errorCode">The optional single code that is expected.</param>
    public GovTestScenarioAttribute(string errorCode = null) {
      if (errorCode != null) ErrorCodes = new string[] { errorCode };
    }

    /// <summary>Creates an instance of the <see cref="GovTestScenarioAttribute"/> class.</summary>
    /// <param name="errorCodes">The error codes that are expected.</param>
    public GovTestScenarioAttribute(string[] errorCodes) {
      ErrorCodes = errorCodes;
    }
  }
}
