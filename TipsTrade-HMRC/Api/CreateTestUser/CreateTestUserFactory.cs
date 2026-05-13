using System;
using System.Linq;
using TipsTrade.HMRC.Api.CreateTestUser.Model;

namespace TipsTrade.HMRC.Api.CreateTestUser {
  /// <summary>
  /// Factory class for creating instances of types that implement <see cref="ICreateTestUserRequest"/>.
  /// </summary>
  /// <remarks>
  /// Provides helper methods to construct request objects and populate their <c>ServiceNames</c> collection
  /// using available service name sources on the request type.
  /// </remarks>
  public class CreateTestUserFactory {
    /// <summary>
    /// Creates a new instance of the specified request type.
    /// </summary>
    /// <typeparam name="T">
    /// The concrete type to create. The type must be a class that implements <see cref="ICreateTestUserRequest"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <typeparamref name="T"/>.
    /// </returns>
    public static T CreateUser<T>() where T : class, ICreateTestUserRequest {
      return Activator.CreateInstance<T>();
    }

    /// <summary>
    /// Creates an <see cref="ICreateTestUserRequest"/> instance of the specified type and populates its
    /// <c>ServiceNames</c> collection with the service names that satisfy the provided predicate.
    /// </summary>
    /// <typeparam name="T">The concrete request type to create. Must implement <see cref="ICreateTestUserRequest"/>.</typeparam>
    /// <param name="predicate">
    /// A filter function used to select service names. The predicate is applied to each available service name;
    /// names that return <c>true</c> are added to the request's <c>ServiceNames</c> collection.
    /// </param>
    /// <returns>
    /// The created and populated <typeparamref name="T"/> instance.
    /// </returns>
    public static T CreateTestUser<T>(Func<string, bool> predicate) where T : class, ICreateTestUserRequest {
      var request = CreateUser<T>();

      request.ServiceNames.AddRange(request.GetServiceNames().Where(predicate));

      return request;
    }

    /// <summary>
    /// Creates an <see cref="ICreateTestUserRequest"/> instance of the specified type and populates its
    /// <c>ServiceNames</c> collection with all available service names.
    /// </summary>
    /// <typeparam name="T">The concrete request type to create. Must implement <see cref="ICreateTestUserRequest"/>.</typeparam>
    /// <returns>
    /// The created and fully populated <typeparamref name="T"/> instance.
    /// </returns>
    public static T CreateTestUserFull<T>() where T : class, ICreateTestUserRequest {
      return CreateTestUser<T>(s => true);
    }
  }
}
