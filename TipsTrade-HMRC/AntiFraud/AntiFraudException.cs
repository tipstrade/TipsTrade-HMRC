using System;
using System.Collections.Generic;

namespace TipsTrade.HMRC.AntiFraud {
  /// <summary>Represents an error throw when the AntiFraud headers fail validation.</summary>
  public class AntiFraudException : Exception {
    /// <summary>Gets or sets the list of errors.</summary>
    public IEnumerable<string> Errors { get; internal set; }

    /// <summary>Creates an instance of the <see cref="AntiFraudException"/> class.</summary>
    public AntiFraudException() {
    }

    /// <summary>Creates an instance of the <see cref="AntiFraudException"/> class.</summary>
    public AntiFraudException(string message) : base(message) {
    }

    /// <summary>Creates an instance of the <see cref="AntiFraudException"/> class.</summary>
    public AntiFraudException(string message, Exception innerException) : base(message, innerException) {
    }
  }
}
