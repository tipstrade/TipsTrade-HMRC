using System;
using System.Collections.Generic;

namespace TipsTrade.HMRC.FraudPrevention {
  /// <summary>Represents an error thrown when fraud prevention headers fail validation.</summary>
  public class FraudPreventionException : Exception {
    /// <summary>Gets the list of validation errors.</summary>
    public IEnumerable<string> Errors { get; internal set; }

    /// <summary>Creates an instance of the <see cref="FraudPreventionException"/> class.</summary>
    public FraudPreventionException() : this(null) {
    }

    /// <summary>Creates an instance of the <see cref="FraudPreventionException"/> class.</summary>
    public FraudPreventionException(string? message) : this(message, null) {
    }

    /// <summary>Creates an instance of the <see cref="FraudPreventionException"/> class.</summary>
    public FraudPreventionException(string? message, Exception? innerException) : base(message, innerException) {
      Errors = new List<string>();
    }
  }
}
