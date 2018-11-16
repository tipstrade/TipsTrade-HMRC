using TipsTrade.HMRC.Api.Model;

namespace TipsTrade.HMRC.HelloWorld.Api.Model {
  /// <summary>Represents a response containing a message.</summary>
  public class MessageResponse : IMessage {
    /// <summary>The response message.</summary>
    public string Message { get; set; }
  }
}
