# TipsTrade-HMRC

[![Nuget](https://img.shields.io/nuget/v/TipsTrade-HMRC.svg)](https://www.nuget.org/packages/TipsTrade-HMRC/)
[![Nuget (Pre-Release)](https://img.shields.io/nuget/vpre/TipsTrade-HMRC.svg)](https://www.nuget.org/packages/TipsTrade-HMRC/)

A strongly typed .NET library for interacting with the HMRC APIs.

The package major version follows the .NET target version (e.g. 8.x.x targets net8.0).

The following services are currently supported:
* `BusinessDetailsMtd`
* `CreateTestUser`
* `HelloWorld`
* `IndividualCalculationsMtd`
* `ObligationsMtd`
* `SelfAssessmentTestSupportMtd`
* `SelfEmploymentBusinessMtd`
* `TestFraudPrevention`
* `Vat`
* `VatNumber`

### Breaking changes

### v.8.x.x

In order to keep the major version in sync with the .NET target version, this release includes major breaking changes. The following changes were made in `8.10.0`:

* Service-based refactor (see below) which removes the old API wrapper layer and introduces service classes for each API.
* `*Api` types have been replaced by `*Service` types (for example, `VatApi` -> `VatService`, `HelloWorldApi` -> `HelloWorldService`).
* Internal API construction via `ApiFactory` and `IClient` has been removed.
* Scope helper calls that referenced API types must now reference service types (for example, `Scopes.GetScopes<VatService>()`).
* Direct integration points that depended on old internal API wrapper classes/interfaces must be updated to use service classes and `HmrcOptions`.
* DI registration is now available via `ServiceCollection` extension methods (`AddHmrc(...)` and per-service add methods).
* The `AntiFraud` namespace and related types have been replaced by the `FraudPrevention` namespace. Fraud prevention is now configured via `HmrcOptions.FraudPreventionConfig` using an `IFraudPrevention` implementation (e.g. `BatchProcessDirect`, `DesktopAppDirect`).
* The `ServerToken` has been removed. Access tokens must now be obtained via `HmrcOAuthService` and an `IHmrcAccessTokenProvider` implementation.
* The `TokenResponse.ExpiresTimestamp` has been removed and replaced by `TokenResponse.CreatedAt` to ensure persistence of the validity period of refresh tokens.

## HMRC Developer Account
A HMRC developer account is required - [Login Here][1]. To make requests you need to create an application which provides:
  * Client ID
  * Client secret

You must configure at least one Redirect URI and add API subscriptions. To run tests and the `Authentication-Client` console app, the solution requires a Redirect URI of `https://www.example.com/hmrc/callback`.

## Creating test users
The solution includes a .NET console app (`Authentication-Client`) which can run in either the Sandbox (default) or Production environment. Before running, populate user-secrets:
```bash
dotnet user-secrets set Production:ClientSecret <Production Client Secret>
dotnet user-secrets set Production:ClientId <Production Client ID>

dotnet user-secrets set Sandbox:ClientSecret <Sandbox Client Secret>
dotnet user-secrets set Sandbox:ClientId <Sandbox Client ID>

dotnet user-secrets -v list
```
By default `Authentication-Client` runs in Sandbox. Pass the `/production` argument to run in Production.

## Test Configuration
Tests require the same credentials as `Authentication-Client`. A `hmrc-users.json` is required (empty by default) and must be populated with the output of `Authentication-Client` for each user type:
```json
{
  "Agent": {
    "User": null,
    "Tokens": null
  },
  "Individual": {
    "User": null,
    "Tokens": null
  },
  "Organisation": {
    "User": null,
    "Tokens": null
  }
}
```

## Scopes
Scopes grant access to specific HMRC APIs and are passed to `HmrcOAuthService.GetAuthorizationEndpoint`. Scopes must also be enabled for your application in the HMRC developer console.

You can reference scope constants directly:
```csharp
var scopes = new string[] { Scopes.Hello, Scopes.VATRead, Scopes.VATWrite };
```

In addition, you can use the `Scopes.GetScopes` helper methods to retrieve scopes for a specific service, or to filter scopes by name or value:
```csharp
// Get all the possible scopes - you probably won't want to do this in production.
var scopes = Scopes.GetScopes();

// Get all the scopes for the specified service
var scopes = Scopes.GetScopes<VatService>();

// All Scopes in the Scopes class that contain "VAT", e.g. "VATRead", "VATWrite"
var scopes = Scopes.GetScopes(nameFilter: (name) => name.Contains("VAT"));

// All scopes in the Scopes class that are for the VAT service, and contain "read"
var scopes = Scopes.GetScopes<VatService>(valueFilter: (value) => value.Contains("read"));
```

## Dependency Injection & Configuration

Services are registered via `IServiceCollection` extension methods. Use `AddHmrc<TAccessTokenProvider>` for full registration, or register individual services with the per-service add methods.

```csharp
var services = new ServiceCollection();

// Register all HMRC services with a custom access token provider
services.AddHmrc<MyAccessTokenProvider>(options => {
  options.ClientId = "Client ID";
  options.ClientSecret = "Client secret";
  options.IsSandbox = true; // omit or set false for production
  options.FraudPreventionConfig = new BatchProcessDirect {
    // populate fraud prevention properties here
  };
}, (client) => {
    client.Timeout = TimeSpan.FromSeconds(30);
});

var provider = services.BuildServiceProvider();
```

Or register services individually:
```csharp
services.AddHmrcOAuthService();
services.AddHmrcHttpClient((client) => {
    client.Timeout = TimeSpan.FromSeconds(30);
});
services.AddSingleton<IHmrcAccessTokenProvider, MyAccessTokenProvider>();
services.AddSingleton(Options.Create(new HmrcOptions
{
    ClientId = "your-client-id",
    ClientSecret = "your-client-secret",
    IsSandbox = true
}));

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
```

## OAuth
To authenticate a user, build the authorization URL and navigate the user to it. After the user authenticates, handle the redirect URI to obtain `TokenResponse` (access + refresh tokens). `HmrcOAuthService` exposes `GetAuthorizationEndpoint` and `HandleEndpointResultAsync` to help with this flow.

Example (console-style):
```csharp
private static async Task<TokenResponse> GetAuthCodeAsync(HmrcOAuthService authClient) {
  var state = $"{Guid.NewGuid()}";
  var scopes = Scopes.GetScopes();
  var redirectUrl = new Uri("https://www.example.com/hmrc/callback");
  var url = authClient.GetAuthorizationEndpoint(state, redirectUrl, scopes);

  Console.WriteLine();
  Console.WriteLine("Navigate to the link below, and login:");
  Console.WriteLine($"\t{url}");

  Console.WriteLine();
  Console.WriteLine($"Paste in the '{redirectUrl}' address that you were redirected to:");
  var redirectedTo = Console.ReadLine();

  Console.WriteLine();
  Console.Write("Validating...");
  var resp = await authClient.HandleEndpointResultAsync(redirectedTo, state);
  Console.WriteLine(" done");

  return resp;
}
```

Refreshing the access token is done via `RefreshAccessTokenAsync`:
```csharp
private static async Task RefreshTokenAsync(HmrcOAuthService authClient) {
  Console.Write("Enter the refresh token: ");
  var refreshToken = Console.ReadLine();
  var tokens = await authClient.RefreshAccessTokenAsync(refreshToken, CancellationToken.None);

  Console.WriteLine(JsonConvert.SerializeObject(tokens, Formatting.Indented));
}
```

## Invoking service methods
Services are resolved from the DI container. Both synchronous and asynchronous method variants are provided (where appropriate):
```csharp
var vatService = serviceProvider.GetRequiredService<VatService>();

// Synchronous
var obligations = vatService.GetObligations(obRequest);

// Asynchronous
var obligations = await vatService.GetObligationsAsync(obRequest);
```

### Exception handling
Service methods can throw the following exceptions:

* `ApiException` — the HMRC API returned an error response. `ApiException.Message` contains the core message; `ApiException.ApiError` may contain `ErrorResponse` objects in `ApiError.Errors`.
* `InvalidOperationException` — thrown if the service configuration is invalid for the request.

Example:
```csharp
try {
  submitResponse = await vatService.SubmitReturnAsync(submitRequest);
} catch (InvalidOperationException ex) {
  var message = ex.Message;
} catch (ApiException ex) {
  var message = ex.Message;
  var detailedMessage = string.Join("\r\n", ex.ApiError.Errors?.Select(x => x.Message));
}
```

### CreateTestUser service
In Sandbox you must create test users regularly (Sandbox data is cleared periodically). Use the `CreateTestUserFactory` helpers:

```csharp
var createTestUser = serviceProvider.GetRequiredService<CreateTestUserService>();

// Create a test organisation user with all the available service types
var orgRequest = CreateTestUserFactory.CreateTestUserFull<CreateOrganisationRequest>();

// Create a test organisation user with only VAT services
var orgVatRequest = CreateTestUserFactory.CreateTestUser<CreateOrganisationRequest>(s => s.Contains("vat"));

var user = await createTestUser.CreateUserAsync(orgRequest);
```

### HelloWorld service
Simple echo endpoints to verify credentials:
```csharp
var helloWorld = serviceProvider.GetRequiredService<HelloWorldService>();

// Returns "Hello World" - no application or user credentials are required
var resp = await helloWorld.SayHelloWorldAsync();

// Returns "Hello Application" - application credentials are required
var resp = await helloWorld.SayHelloApplicationAsync();

// Returns "Hello User" - a valid user's access token is required
var resp = await helloWorld.SayHelloUserAsync();
```

### VAT (MTD) service

#### Get obligations
VAT returns are indexed by a "Period Key". `ObligationsResult` implements `IComparable` so results can be sorted. Note: HMRC specify that the Period Key should not be shown to end users.
```csharp
var vatService = serviceProvider.GetRequiredService<VatService>();

var obRequest = new ObligationsRequest() {
  Vrn = vrn,
  Status = ObligationStatus.Fulfilled, // Use null to request all obligations
  DateFrom = DateTime.Today.AddYears(-1),
  DateTo = DateTime.Today
};

var obligations = await vatService.GetObligationsAsync(obRequest);

// The most recent fulfilled obligation
var periodKey = obligations.Value?.OrderByDescending(x => x).FirstOrDefault()?.PeriodKey;
```

#### Get VAT return
```csharp
var returnRequest = new ReturnRequest() {
  Vrn = vrn,
  PeriodKey = periodKey // As retrieved by GetObligations
};

var vatReturn = await vatService.GetReturnAsync(returnRequest);
```

#### Submit VAT return
Decimal values should have no more than two decimal places (use `Math.Round`).
```csharp
var vatReturn = new VatReturn() {
  PeriodKey = periodKey,
  VatDueSales = 7724.92m,
  VatDueAcquisitions = 703.49m,
  TotalVatDue = 7724.92m + 703.49m,
  VatReclaimedCurrPeriod = 1681.08m,
  NetVatDue = 7724.92m + 703.49m - 1681.08m,
  TotalValueSalesExVAT = 38622,
  TotalValuePurchasesExVAT = 8405,
  TotalValueGoodsSuppliedExVAT = 3703,
  TotalAcquisitionsExVAT = 500,
  Finalised = true // Must be true for the return to be accepted
};

var request = new SubmitRequest() {
  Return = vatReturn,
  Vrn = vrn,
};

var resp = await vatService.SubmitReturnAsync(request);
```

#### Other methods
Other VAT methods: `GetLiabilities`, `GetPayments` - see API docs in code.

## Fraud prevention headers
Certain services require fraud prevention headers. Configure them via `HmrcOptions.FraudPreventionConfig` using an `IFraudPrevention` implementation from the `TipsTrade.HMRC.FraudPrevention.ConnectionMethods` namespace.

Available connection method types:
* `BatchProcessDirect`
* `DesktopAppDirect`
* `DesktopAppViaServer`
* `MobileAppDirect`
* `MobileAppViaServer`
* `OtherDirect`
* `OtherViaServer`
* `WebAppViaServer`

Each type implements one or more header interfaces (e.g. `IDeviceId`, `ILocalIps`, `IMacAddresses`, `IUserAgent`, `IScreens`, `ITimeZone`, `IVendorVersion`, `IWindowSize`) which expose the relevant properties. Helper extension methods are available on several interfaces:

```csharp
// Populates with all the local IP addresses on the local system
localIps.PopulateLocalIps();

// Populates with all the local IP addresses that match a predicate
localIps.PopulateLocalIps(ip => ip.AddressFamily == AddressFamily.InterNetwork);

// Populates with all the MAC addresses on the local system
macAddresses.PopulateMacAddresses();

// Populates with the local operating system name and version, and device details
userAgent.PopulateUserAgent("Dell", "XPS");
```

Example configuration:
```csharp
options.FraudPreventionConfig = new BatchProcessDirect {
  DeviceId = Guid.Parse("your-device-guid"),
  TimeZone = TimeZoneInfo.Local,
  VendorVersion = new Dictionary<string, string>() {
    { "MyApp", "1.0.0.0" }
  }
};

// Populate system values via interface checks
if (options.FraudPreventionConfig is ILocalIps localIps)
  localIps.PopulateLocalIps();

if (options.FraudPreventionConfig is IMacAddresses macAddresses)
  macAddresses.PopulateMacAddresses();

if (options.FraudPreventionConfig is IUserAgent userAgent)
  userAgent.PopulateUserAgent("MyManufacturer", "MyProduct");
```

### TestFraudPrevention service
The `TestFraudPreventionService` can be used in Sandbox to validate your fraud prevention headers against HMRC's spec, and to retrieve per-service feedback.

```csharp
var testFraudPrevention = serviceProvider.GetRequiredService<TestFraudPreventionService>();

// Validate the currently configured fraud prevention headers
var result = await testFraudPrevention.ValidateAsync();

if (result.Errors?.Any() == true) {
  // Headers are invalid - inspect result.Errors for details
}

// Get feedback for a specific service (e.g. "vat-mtd")
var feedback = await testFraudPrevention.GetFeedbackAsync("vat-mtd");
```

[1]: https://developer.service.hmrc.gov.uk/developer/login
[2]: https://developer.service.hmrc.gov.uk/developer/applications
[3]: https://developer.service.hmrc.gov.uk/api-documentation/docs/testing/data-cleardown
[4]: https://developer.service.hmrc.gov.uk/api-documentation/docs/fraud-prevention