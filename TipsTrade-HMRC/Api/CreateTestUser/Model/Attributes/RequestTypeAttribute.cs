using System;

namespace TipsTrade.HMRC.Api.CreateTestUser.Model.Attributes {
  /// <summary>Represents an attribute that indicates that the request type used to create the class.</summary>
  [AttributeUsage(AttributeTargets.Class)]
  internal class RequestTypeAttribute :Attribute {
    internal Type RequestType { get; }

    internal RequestTypeAttribute(Type type) {
      RequestType = type;
    }
  }
}
