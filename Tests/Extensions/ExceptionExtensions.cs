using System;
using System.Diagnostics.CodeAnalysis;

namespace TipsTrade.HMRC.Tests.Extensions {
  internal static class ExceptionExtensions {
    /// <summary>
    /// Recursively checks the exception and its inner exceptions for a message that contains the specified expected message.
    /// </summary>
    /// <param name="ex">The exception to check.</param>
    /// <param name="expectedMessage">The expected message to look for.</param>
    /// <returns>True if the expected message is found; otherwise, false.</returns>
    public static bool ExceptionContains(this Exception ex, string expectedMessage) {
      return ex.ExceptionContains(e => e.Message.Contains(expectedMessage, StringComparison.OrdinalIgnoreCase), out _);
    }

    /// <summary>
    /// Recursively checks the exception and its inner exceptions for a message that contains the specified expected message.
    /// </summary>
    /// <param name="ex">The exception to check.</param>
    /// <param name="expectedMessage">The expected message to look for.</param>
    /// <param name="innerException">The inner exception that contains the expected message, if found.</param>
    /// <returns>True if the expected message is found; otherwise, false.</returns>
    public static bool ExceptionContains(this Exception ex, string expectedMessage, [NotNullWhen(true)] out Exception? innerException) {
      return ex.ExceptionContains(e => e.Message.Contains(expectedMessage, StringComparison.OrdinalIgnoreCase), out innerException);
    }

    /// <summary>
    /// Recursively checks the exception and its inner exceptions for a message that satisfies the specified predicate.
    /// </summary>
    /// <param name="ex">The exception to check.</param>
    /// <param name="expectedMessagePredicate">The predicate to evaluate the exception message.</param>
    /// <returns>True if the expected message is found; otherwise, false.</returns>
    public static bool ExceptionContains(this Exception ex, Func<Exception, bool> expectedMessagePredicate) {
      return ex.ExceptionContains(expectedMessagePredicate, out _);
    }

    /// <summary>
    /// Recursively checks the exception and its inner exceptions for a message that contains the specified expected message.
    /// </summary>
    /// <param name="ex">The exception to check.</param>
    /// <param name="expectedMessagePredicate">The predicate to evaluate the exception message.</param>
    /// <param name="innerException">The inner exception that contains the expected message, if found.</param>
    /// <returns>True if the expected message is found; otherwise, false.</returns>
    public static bool ExceptionContains(this Exception ex, Func<Exception, bool> expectedMessagePredicate, [NotNullWhen(true)] out Exception? innerException) {
      if (expectedMessagePredicate(ex)) {
        innerException = ex;
        return true;
      } else if (ex.InnerException != null) {
        return ex.InnerException.ExceptionContains(expectedMessagePredicate, out innerException);
      }

      innerException = null;
      return false;
    }
  }
}
