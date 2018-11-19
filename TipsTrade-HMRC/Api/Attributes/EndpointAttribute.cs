using System;

namespace TipsTrade.HMRC.Api.Attributes {
  /// <summary>Represents an attribute that indicates that the endpoint for the class.</summary>
  [AttributeUsage(AttributeTargets.Class)]
  internal class EndpointAttribute : Attribute {
    internal string Endpoint { get; }

    internal EndpointAttribute(string endpoint) {
      Endpoint = endpoint;
    }
  }
}
