# DuoSecurity.Auth.Http
A .NET HttpClient wrapper for interacting with the Duo Security Auth REST API.

# Installation
```
NuGet PM> Install-Package DuoSecurity.Auth.Http
```

# Usage

### Client Setup

```C#
using DuoSecurity.Auth.Http;

// Provide Config
var config = new DuoAuthConfig("api-XXXXXXXX.duosecurity.com", "integrationKey", "secretKey");

// Instantiate Client
using var client = new DuoAuthClient(config);

// Make Requests..
```

Note that the client constructor has an overload that takes an `HttpClient` so that you may
use [HTTP client injection](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#how-to-use-typed-clients-with-ihttpclientfactory) when configuring your dependency pool.

---

### All Endpoints Are Supported

| Endpoint | Method               |
|----------|----------------------|
| /ping | PingAsync            |
| /check | CheckAsync           |
| /logo | LogoAsync            |
| /enroll | EnrollAsync          |
| /enroll_status | EnrollStatusAsync    |
| /preauth | PreAuthAsync         |
| /auth | AuthAsync            |
| /auth (async) | AuthWithPollingAsync |
| /auth_status | AuthStatusAsync      |

### All Factors Are Supported

Factors are supported via derivations of the `AuthRequest` object.

The object(s) can be derived from to support new factors or customize parameters sent to the API.

The objects are:
- `AutoAuthRequest`
- `PushAuthRequest`
- `PasscodeAuthRequest`
- `PhoneAuthRequest`
- `SmsAuthRequest`