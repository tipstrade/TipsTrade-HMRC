using System;

namespace TipsTrade.HMRC.Api.Model.Attributes {
  /// <summary>Represents an attribute that indicates that the field is a Gov Test Scenario.</summary>
  [AttributeUsage(AttributeTargets.Field)]
  public class GovTestScenarioAttribute : Attribute {
  }
}
