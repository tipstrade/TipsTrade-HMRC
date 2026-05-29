using Microsoft.Extensions.DependencyInjection;
using System;
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
    /// <summary>
    /// Registers <see cref="HmrcOptions"/> and all available HMRC API services with the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configure">A delegate to configure the <see cref="HmrcOptions"/>.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so calls can be chained.</returns>
    public static IServiceCollection AddHmrc(this IServiceCollection services, Action<HmrcOptions> configure) {
      if (services  == null) {
        throw new ArgumentNullException(nameof(services));
      } else if (configure == null) {
        throw new ArgumentNullException(nameof(configure));
      }

      services.Configure(configure);

      services.AddHttpClient(Api.HmrcServiceBase.HttpClientName);

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

    /// <summary>Registers <see cref="HmrcOAuthService"/> with the <see cref="IServiceCollection"/>.</summary>
    public static IServiceCollection AddHmrcOAuthService(this IServiceCollection services) {
      return services.AddTransient<HmrcOAuthService>();
    }

    /// <summary>Registers <see cref="BusinessDetailsMtdService"/> with the <see cref="IServiceCollection"/>.</summary>
    public static IServiceCollection AddBusinessDetailsMtdService(this IServiceCollection services) {
      return services.AddTransient<BusinessDetailsMtdService>();
    }

    /// <summary>Registers <see cref="CreateTestUserService"/> with the <see cref="IServiceCollection"/>.</summary>
    public static IServiceCollection AddCreateTestUserService(this IServiceCollection services) {
      return services.AddTransient<CreateTestUserService>();
    }

    /// <summary>Registers <see cref="HelloWorldService"/> with the <see cref="IServiceCollection"/>.</summary>
    public static IServiceCollection AddHelloWorldService(this IServiceCollection services) {
      return services.AddTransient<HelloWorldService>();
    }

    /// <summary>Registers <see cref="IndividualCalculationsMtdService"/> with the <see cref="IServiceCollection"/>.</summary>
    public static IServiceCollection AddIndividualCalculationsMtdService(this IServiceCollection services) {
      return services.AddTransient<IndividualCalculationsMtdService>();
    }

    /// <summary>Registers <see cref="ObligationsMtdService"/> with the <see cref="IServiceCollection"/>.</summary>
    public static IServiceCollection AddObligationsMtdService(this IServiceCollection services) {
      return services.AddTransient<ObligationsMtdService>();
    }

    /// <summary>Registers <see cref="SelfAssessmentTestSupportMtdService"/> with the <see cref="IServiceCollection"/>.</summary>
    public static IServiceCollection AddSelfAssessmentTestSupportMtdService(this IServiceCollection services) {
      return services.AddTransient<SelfAssessmentTestSupportMtdService>();
    }

    /// <summary>Registers <see cref="SelfEmploymentBusinessMtdService"/> with the <see cref="IServiceCollection"/>.</summary>
    public static IServiceCollection AddSelfEmploymentBusinessMtdService(this IServiceCollection services) {
      return services.AddTransient<SelfEmploymentBusinessMtdService>();
    }

    /// <summary>Registers <see cref="TestFraudPreventionService"/> with the <see cref="IServiceCollection"/>.</summary>
    public static IServiceCollection AddTestFraudPreventionService(this IServiceCollection services) {
      return services.AddTransient<TestFraudPreventionService>();
    }

    /// <summary>Registers <see cref="VatService"/> with the <see cref="IServiceCollection"/>.</summary>
    public static IServiceCollection AddVatService(this IServiceCollection services) {
      return services.AddTransient<VatService>();
    }

    /// <summary>Registers <see cref="VatNumberService"/> with the <see cref="IServiceCollection"/>.</summary>
    public static IServiceCollection AddVatNumberService(this IServiceCollection services) {
      return services.AddTransient<VatNumberService>();
    }
  }
}
