using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using TipsTrade.HMRC.Api.BusinessDetailsMtd;
using TipsTrade.HMRC.Api.OAuth;
using TipsTrade.HMRC.Api.CreateTestUser;
using TipsTrade.HMRC.Api.HelloWorld;
using TipsTrade.HMRC.Api.IndividualCalculationsMtd;
using TipsTrade.HMRC.Api.ObligationsMtd;
using TipsTrade.HMRC.Api.SelfAssessmentTestSupportMtd;
using TipsTrade.HMRC.Api.SelfEmploymentBusinessMtd;
using TipsTrade.HMRC.Api.TestFraudPrevention;
using TipsTrade.HMRC.Api.Vat;

namespace TipsTrade.HMRC.Extensions {
  /// <summary>Extension methods for registering HMRC API services with an <see cref="IServiceCollection"/>.</summary>
  public static class ServiceCollectionExtensions {
    /// <summary>The name used to register the named <see cref="System.Net.Http.HttpClient"/> for HMRC API calls.</summary>
    public static readonly string HttpClientName = typeof(Api.HmrcServiceBase).FullName ?? typeof(Api.HmrcServiceBase).Name;
    /// <summary>
    /// Registers HMRC API services with the <see cref="IServiceCollection"/> using the specified access token provider and tenant provider.
    /// </summary>
    /// <typeparam name="TAccessTokenProvider">The type of the access token provider.</typeparam>
    /// <typeparam name="TTenantProvider">The type of the tenant provider.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">The action to configure HMRC options.</param>
    /// <param name="configureClient">The action to configure the HTTP client.</param>
    /// <returns>The updated service collection.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddHmrc<TAccessTokenProvider, TTenantProvider>(this IServiceCollection services, Action<HmrcOptions> configureOptions, Action<HttpClient>? configureClient = null)
      where TAccessTokenProvider : class, IHmrcAccessTokenProvider
      where TTenantProvider : class, IHmrcTenantProvider {
      if (services == null) {
        throw new ArgumentNullException(nameof(services));
      }

      return services
        .AddHmrcTenantProvider<TTenantProvider>()
        .AddHmrc<TAccessTokenProvider>(configureOptions, configureClient);
    }

    /// <summary>
    /// Registers HMRC API services with the <see cref="IServiceCollection"/> using the specified access token provider, tenant provider, and options provider.
    /// </summary>
    /// <typeparam name="TAccessTokenProvider">The type of the access token provider.</typeparam>
    /// <typeparam name="TTenantProvider">The type of the tenant provider.</typeparam>
    /// <typeparam name="TOptionsProvider">The type of the options provider.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configureClient">The action to configure the HTTP client.</param>
    /// <returns>The updated service collection.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddHmrc<TAccessTokenProvider, TTenantProvider, TOptionsProvider>(this IServiceCollection services, Action<HttpClient>? configureClient = null)
      where TAccessTokenProvider : class, IHmrcAccessTokenProvider
      where TOptionsProvider : class, IHmrcOptionsProvider
      where TTenantProvider : class, IHmrcTenantProvider {
      if (services == null) {
        throw new ArgumentNullException(nameof(services));
      }

      return services
        .AddHmrcTenantProvider<TTenantProvider>()
        .AddHmrc<TAccessTokenProvider, TOptionsProvider>(configureClient);
    }

    /// <summary>
    /// Registers HMRC API services with the <see cref="IServiceCollection"/> using the specified options provider and access token provider.
    /// </summary>
    /// <typeparam name="TAccessTokenProvider">The type of the access token provider.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configureOptions">An action to configure the HMRC options.</param>
    /// <param name="configureClient">An optional action to configure the <see cref="System.Net.Http.HttpClient"/>. If null, a default client will be registered.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so calls can be chained.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddHmrc<TAccessTokenProvider>(this IServiceCollection services, Action<HmrcOptions> configureOptions, Action<HttpClient>? configureClient = null)
        where TAccessTokenProvider : class, IHmrcAccessTokenProvider {
      if (services == null) {
        throw new ArgumentNullException(nameof(services));
      } else if (configureOptions == null) {
        throw new ArgumentNullException(nameof(configureOptions));
      }

      services.Configure(configureOptions);

      return services.AddHmrc<TAccessTokenProvider, HmrcOptionsProvider>(configureClient);
    }

    /// <summary>
    /// Registers HMRC API services with the <see cref="IServiceCollection"/> using the specified access token provider and options provider.
    /// </summary>
    /// <typeparam name="TAccessTokenProvider">The type of the access token provider.</typeparam>
    /// <typeparam name="TOptionsProvider">The type of the options provider.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configureClient">The action to configure the HTTP client.</param>
    /// <returns>The updated service collection.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <remarks>All services apart from the <see cref="Api.ApplicationTokenCache"/> are registered as scoped.</remarks>
    public static IServiceCollection AddHmrc<TAccessTokenProvider, TOptionsProvider>(this IServiceCollection services, Action<HttpClient>? configureClient = null)
        where TAccessTokenProvider : class, IHmrcAccessTokenProvider
        where TOptionsProvider : class, IHmrcOptionsProvider {
      if (services == null) {
        throw new ArgumentNullException(nameof(services));
      }

      services.AddHmrcHttpClient(configureClient);

      // ApplicationTokenCache is registered as a singleton to ensure that access tokens are cached across the entire application,
      // allowing for efficient reuse of tokens and reducing the number of token requests made to the HMRC API.
      services.AddSingleton<Api.ApplicationTokenCache>();

      // Everything else is scoped to ensure that each HTTP request gets a new instance of the access token provider, options provider, and tenant provider,
      services.AddHmrcAccessTokenProvider<TAccessTokenProvider>();
      services.AddHmrcOptionsProvider<TOptionsProvider>();

      services.AddHmrcOAuthService();
      services.AddBusinessDetailsMtdService();
      services.AddCreateTestUserService();
      services.AddHelloWorldService();
      services.AddIndividualCalculationsMtdService();
      services.AddObligationsMtdService();
      services.AddSelfAssessmentTestSupportMtdService();
      services.AddSelfEmploymentBusinessMtdService();
      services.AddTestFraudPreventionService();
      services.AddVatService();
      services.AddVatNumberService();

      return services;
    }

    /// <summary>
    /// Registers a custom implementation of <see cref="IHmrcAccessTokenProvider"/> with the <see cref="IServiceCollection"/> as a scoped service.
    /// </summary>
    /// <typeparam name="T">The type of the access token provider.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so calls can be chained.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddHmrcAccessTokenProvider<T>(this IServiceCollection services) where T : class, IHmrcAccessTokenProvider {
      if (services == null) {
        throw new ArgumentNullException(nameof(services));
      }

      return services.AddScoped<IHmrcAccessTokenProvider, T>();
    }

    /// <summary>
    /// Registers a named <see cref="System.Net.Http.HttpClient"/> with the <see cref="IServiceCollection"/> for use with HMRC API calls, using the specified configuration action.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configureClient">An optional action to configure the <see cref="System.Net.Http.HttpClient"/>. If null, a default client will be registered.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so calls can be chained.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddHmrcHttpClient(this IServiceCollection services, Action<HttpClient>? configureClient = null) {
      if (services == null) {
        throw new ArgumentNullException(nameof(services));
      }

      configureClient ??= (_) => { };

      services.AddHttpClient(HttpClientName, configureClient);

      return services;
    }

    /// <summary>
    /// Registers a custom implementation of <see cref="IHmrcOptionsProvider"/> with the <see cref="IServiceCollection"/> as a scoped service.
    /// </summary>
    /// <typeparam name="T">The type of the options provider.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so calls can be chained.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddHmrcOptionsProvider<T>(this IServiceCollection services) where T : class, IHmrcOptionsProvider {
      if (services == null) {
        throw new ArgumentNullException(nameof(services));
      }

      services.AddScoped<IHmrcOptionsProvider, T>();

      return services;
    }

    /// <summary>
    /// Registers a custom implementation of <see cref="IHmrcTenantProvider"/> with the <see cref="IServiceCollection"/> as a scoped service.
    /// This allows you to provide tenant information for multi-tenant applications when using the HMRC API services.
    /// </summary>
    /// <typeparam name="T">The type of the tenant provider.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so calls can be chained.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddHmrcTenantProvider<T>(this IServiceCollection services) where T : class, IHmrcTenantProvider {
      if (services == null) {
        throw new ArgumentNullException(nameof(services));
      }

      services.AddScoped<IHmrcTenantProvider, T>();

      return services;
    }

    /// <summary>Registers <see cref="HmrcOAuthService"/> with the <see cref="IServiceCollection"/> as a scoped service.</summary>
    public static IServiceCollection AddHmrcOAuthService(this IServiceCollection services) {
      return services.AddScoped<HmrcOAuthService>();
    }

    /// <summary>Registers <see cref="BusinessDetailsMtdService"/> with the <see cref="IServiceCollection"/> as a scoped service.</summary>
    public static IServiceCollection AddBusinessDetailsMtdService(this IServiceCollection services) {
      return services.AddScoped<BusinessDetailsMtdService>();
    }

    /// <summary>Registers <see cref="CreateTestUserService"/> with the <see cref="IServiceCollection"/> as a scoped service.</summary>
    public static IServiceCollection AddCreateTestUserService(this IServiceCollection services) {
      return services.AddScoped<CreateTestUserService>();
    }

    /// <summary>Registers <see cref="HelloWorldService"/> with the <see cref="IServiceCollection"/> as a scoped service.</summary>
    public static IServiceCollection AddHelloWorldService(this IServiceCollection services) {
      return services.AddScoped<HelloWorldService>();
    }

    /// <summary>Registers <see cref="IndividualCalculationsMtdService"/> with the <see cref="IServiceCollection"/> as a scoped service.</summary>
    public static IServiceCollection AddIndividualCalculationsMtdService(this IServiceCollection services) {
      return services.AddScoped<IndividualCalculationsMtdService>();
    }

    /// <summary>Registers <see cref="ObligationsMtdService"/> with the <see cref="IServiceCollection"/> as a scoped service.</summary>
    public static IServiceCollection AddObligationsMtdService(this IServiceCollection services) {
      return services.AddScoped<ObligationsMtdService>();
    }

    /// <summary>Registers <see cref="SelfAssessmentTestSupportMtdService"/> with the <see cref="IServiceCollection"/> as a scoped service.</summary>
    public static IServiceCollection AddSelfAssessmentTestSupportMtdService(this IServiceCollection services) {
      return services.AddScoped<SelfAssessmentTestSupportMtdService>();
    }

    /// <summary>Registers <see cref="SelfEmploymentBusinessMtdService"/> with the <see cref="IServiceCollection"/> as a scoped service.</summary>
    public static IServiceCollection AddSelfEmploymentBusinessMtdService(this IServiceCollection services) {
      return services.AddScoped<SelfEmploymentBusinessMtdService>();
    }

    /// <summary>Registers <see cref="TestFraudPreventionService"/> with the <see cref="IServiceCollection"/> as a scoped service.</summary>
    public static IServiceCollection AddTestFraudPreventionService(this IServiceCollection services) {
      return services.AddScoped<TestFraudPreventionService>();
    }

    /// <summary>Registers <see cref="VatService"/> with the <see cref="IServiceCollection"/> as a scoped service.</summary>
    public static IServiceCollection AddVatService(this IServiceCollection services) {
      return services.AddScoped<VatService>();
    }

    /// <summary>Registers <see cref="VatNumberService"/> with the <see cref="IServiceCollection"/> as a scoped service.</summary>
    public static IServiceCollection AddVatNumberService(this IServiceCollection services) {
      return services.AddScoped<VatNumberService>();
    }
  }
}
