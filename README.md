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
var client = new DuoAuthClient(config);
```

---

### All Endpoints Are Supported

| Endpoint |  Method |
|----------|---------|
| /ping | PingAsync |
| /check | CheckAsync |
| /logo | LogoAsync |
| /enroll | EnrollAsync |
| /enroll_status | EnrollStatusAsync |
| /preauth | PreAuthBy{**UserKey**}Async |
| /auth | Auth{**Factor**}By{**UserKey**}Async |
| /auth (async) | Auth{**Factor**}By{**UserKey**}ForPollingAsync |
| /auth_status | AuthStatusAsync |

**{UserKey}** can be substituted for *UserId* or *Username.*

**{Factor}** can be substituted for one of the follow: *Auto*, *Push*, *Passcode* , *Phone*, or *SMS.*
