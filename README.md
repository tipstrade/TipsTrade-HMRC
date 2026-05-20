# TipsTrade-HMRC

A strongly typed .NET client for interacting with the HMRC APIs.

Package version: 0.0.4-beta1 - targets `net8` and `net481`. This release adds async support and the Test Fraud Prevention / fraud feedback APIs.

The following APIs are currently supported (available from the `Client` properties):
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
Whilst I make an effort not to introduce breaking changes (I will attempt to decorate deprecated items with the `ObsoleteAttribute` when possible), because several HMRC APIs are still in Beta (and because of mistakes I've made), breaking changes will occur. I will document them here.

#### v.0.0.1.9
* `IDateRange` properties renamed from `From` and `To` to `DateFrom` and `DateTo`.
* An `AntiFraudException` is now thrown instead of an `InvalidOperationException` when the AntiFraud headers fail validation. `AntiFraudException` contains an `Errors` property with all validation errors.

Release notes for `0.0.4-beta1` include:
* Add async support (async API method variants available).
* Add fraud feedback / Test Fraud Prevention API.
* Fix: missing fraud prevention headers on some API calls.

## HMRC Developer Account
A HMRC developer account is required - [Login Here][1]. To make requests you need to create an application which provides:
  * Client ID
  * Client secret

You must configure at least one Redirect URI and add API subscriptions. To run tests and the `Authentication-Client` console app, the solution requires a Redirect URI of `https://www.example.com/hmrc/callback`.

## Creating test users
The solution includes a .NET console app (`Authentication-Client`) which can run in either the Sandbox (default) or Production environment. Before running, populate user-secrets:
```bash
dotnet user-secrets set Production:ServerToken <Sandbox Server Token>
dotnet user-secrets set Production:ClientSecret <Sandbox Client Secret>
dotnet user-secrets set Production:ClientID <Sandbox Client ID>

dotnet user-secrets set Sandbox:ServerToken <Sandbox Server Token>
dotnet user-secrets set Sandbox:ClientSecret <Sandbox Client Secret>
dotnet user-secrets set Sandbox:ClientID <Sandbox Client ID>

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
Scopes grant access to specific HMRC APIs and are passed to `Client.GetAuthorizationEndpoint`. Scopes must also be enabled for your application in the HMRC developer console.

You can reference scope constants directly:
```C#
var scopes = new string[] {Scopes.Hello, Scopes.VATRead, Scopes.VATWrite};
```

In addition, the you can use the `Scopes.GetScopes` helper methods to retrieve scopes for a specific Api, or to filter scopes by name or value:
```C#
// Get all the possible scopes - you probably won't want to do this in production.
var scopes = Scopes.GetScopes();

// Get all the scopes for the specified Api
var scopes = Scopes.GetScopes<Api.Vat.VatApi>();

// All Scopes in the Scopes class that contains "VAT", eg. "VATRead", "VATWrite"
var scopes = Scopes.GetScopes(nameFilter: (name) => name.Contains("VAT"));

// All scopes in the Scopes class that are for the VAT api, and contain "read"
var scopes = Scopes.GetScopes<Api.Vat.VatApi>(valueFilter: (value) => value.Contains("read"));
```

## Creating the Api Client
The `Client` class requires credentials before calling most API methods.
```C#
// Creates a client with production credentials
var client = new Client("Client ID", "Client secret");

// Creates a client with sandbox credentials
var client = new Client("Client ID", "Client secret", true);

// Or using the properties
var client = new Client() {
  ClientID = "Client ID",
  ClientSecret = "Client secret",
  // ServerToken is deprecated and will be removed
  IsSandbox = true
};
```

## OAuth
To authenticate a user, build the authorization URL and navigate the user to it. After the user authenticates, handle the redirect URI to obtain
`TokenResponse` (access + refresh tokens). The library exposes `GetAuthorizationEndpoint` and `HandleEndpointResult` to help with this flow.

Example (console-style):
```C#
private static TokenResponse GetAuthCode(Client client) {
  var state = $"{Guid.NewGuid()}";
  var scopes = Scopes.GetScopes();
  var redirectUrl = Configuration["RedirectUrl"];
  var url = client.GetAuthorizationEndpoint(state, redirectUrl, scopes);

  Console.WriteLine();
  Console.WriteLine("Navigate to the link below, and login:");
  Console.WriteLine($"\t{url}");

  Console.WriteLine();
  Console.WriteLine($"Paste in the '{redirectUrl}' address that you were redirected to:");
  var redirectedTo = Console.ReadLine();

  Console.WriteLine();
  Console.Write("Validating...");
  var resp = client.HandleEndpointResult(redirectedTo, state);
  Console.WriteLine(" done");

  Console.WriteLine();
  Console.ForegroundColor = ConsoleColor.Green;
  Console.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
  Console.ResetColor();
  Console.Write("");

  return resp;
}
```

Refreshing the access token is done by simply passing the stored refresh token to the Client.RefreshAccessToken method:

```C#
private static void RefreshToken(Client client) {
  Console.WriteLine();
  Console.Write("Enter the refresh token: ");
  var refreshToken = Console.ReadLine();
  var tokens = client.RefreshAccessToken(refreshToken);

  Console.WriteLine();
  Console.ForegroundColor = ConsoleColor.Green;
  Console.WriteLine(JsonConvert.SerializeObject(tokens, Formatting.Indented));
  Console.ResetColor();
  Console.Write("");
}
```

Async variants of token and API calls are available in this release.

## Invoking API methods
APIs are available as properties on the `Client` instance (see list at the top). Both synchronous and asynchronous method variants are provided (where appropriate),
e.g., `client.Vat.GetObligations(...)` and `client.Vat.GetObligationsAsync(...)`.

### Exception handling
API methods can throw `ApiException`. `ApiException.Message` contains the core message; `ApiException.ApiError` may contain `ErrorResponse` objects in `ApiError.Errors`.

`InvalidOperationException` may be thrown if the `Client` state is invalid for the request.

APIs that require AntiFraud headers will throw `AntiFraudException` if headers are missing/invalid. `AntiFraudException.Errors` contains all validation messages.

Example:
```C#
try {
  submitResponse = await client.Vat.SubmitReturnAsync(submitRequest);
  // ...
} catch (InvalidOperationException ex) {
  var message = ex.Message;
} catch (ApiException ex) {
  var message = ex.Message;
  var detailedMessage = string.Join("\r\n", ex.ApiError.Errors?.Select(x => x.Message));
} catch (AntiFraudException ex) {
  var detailedMessage = string.Join("\r\n", ex.Errors);
}
```

### CreateTestUser API
In Sandbox you must create test users regularly (Sandbox data is cleared periodically). Use the `CreateTestUserFactory` helpers:

```C#
// Create a test organisation user with all the available service types
var orgRequest = CreateTestUserFactory.CreateTestUserFull<CreateOrganisationRequest>();
 
// Create a test organisation user with only VAT services
var orgVatRequest = CreateTestUserFactory.CreateTestUser<CreateOrganisationRequest>(s => s.Contains("vat"));

var user = client.CreateTestUser.CreateUser(orgRequest);
```

### HelloWorld API
Simple echo endpoints to verify credentials:
```C#
// Returns "Hello World" - no application or user credentials are required
var resp = client.HelloWorld.SayHelloWorld();

// Returns "Hello Application" - application credentials are required
var resp = client.HelloWorld.SayHelloApplication();

// Returns "Hello User" - a valid user's access token is required
var tokens = client.RefreshAccessToken("Saved Refresh Token");
var resp = client.HelloWorld.SayHelloUser();
```

### VAT (MTD) API

#### Get obligations
VAT returns are indexed by a "Period Key". `ObligationsResult` implements `IComparable` so results can be sorted. Note: HMRC specify that the Period Key should not be shown to end users.
```C#
// Get all fulfilled obligations for the past year
var obRequest = new ObligationsRequest() {
  Vrn = vrn,
  Status = ObligationStatus.Fulfilled, // Use null to request all obligations
  DateFrom = DateTime.Today.AddYears(-1),
  DateTo = DateTime.Today
};

var obligations = client.Vat.GetObligations(obRequest);

// The most recent fulfilled obligation
var periodKey = resp.Value?.OrderByDescending(x => x).FirstOrDefault()?.PeriodKey;
```

#### Get VAT return
```C#
var returnRequest = new ReturnRequest() {
  Vrn = vrn,
  PeriodKey = periodKey // As retrieved by GetObligations
};

var vatReturn = client.Vat.GetReturn(returnRequest);
```

#### Submit VAT return
Decimal values should have no more than two decimal places (use `Math.Round`).
```C#
var vatReturn = new VatReturn() {
  PeriodKey = periodKey, // As retrieved by GetObligations
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

var resp = client.Vat.SubmitReturn(request);
```

#### Other methods
Other VAT methods: `GetLiabilities`, `GetPayments` - see API docs in code.

## Fraud prevention headers
Certain APIs require fraud-prevention headers. Any API implementing `IRequiresAntiFraud` expects `Client.AntiFraud` to be populated. Missing or invalid headers throw `AntiFraudException` with `Errors`.

The `AntiFraud` helper includes:
```C#
// Returns a list of all the properties that are required for a certain ConnectionMethod
var props = AntiFraud.AntiFraud.GetPropertiesForMethod(ConnectionMethod.DESKTOP_APP_DIRECT);

// Validate the AntiFraud headers object
var isValid = client.AntiFraud.Validate();

// Validate the AntiFraud headers object, passing out a list of the actual errors
var isValid = client.AntiFraud.Validate(out string[] errors);

// Populates with all the local IP addresses on the local system
client.AntiFraud.PopulateLocalIPs();

// Populates with all the MAC addresses on the local system
client.AntiFraud.PopulateMACAddresses();

// Populates with the local operating system name and version
client.AntiFraud.PopulateUserAgent();

// Populates with all the screen information on the local system (.Net Framework Only)
client.AntiFraud.PopulateScreens();
```

Example usage:
```C#
client.AntiFraud = new AntiFraud.AntiFraud() {
  ConnectionMethod = ConnectionMethod.BATCH_PROCESS_DIRECT,
  DeviceID = Configuration["AntiFraudDeviceID"],
  Screens = new Screen[] {
    new Screen() {ColourDepth = 32, ScalingFactor=1, Size = new Size(1920,1080) }
  },
  TimeZone = TimeZoneInfo.Local,
  VendorVersion = new Dictionary<string, string>() { { "TipsTrade.HMRC.Tests", "0.0.0.1" } },
  WindowSize = new Size(1024, 768)
}
```

[1]: https://developer.service.hmrc.gov.uk/developer/login
[2]: https://developer.service.hmrc.gov.uk/developer/applications
[3]: https://developer.service.hmrc.gov.uk/api-documentation/docs/testing/data-cleardown
[4]: https://developer.service.hmrc.gov.uk/api-documentation/docs/fraud-prevention
