### Breaking changes from pre-release to release

1. Moved **DuoResponse** type from *DuoSecurity.Auth.Http.Core* namespace to *DuoSecurity.Auth.Http* to cut down on *using*
calls necessary for the library. Most will probably just use *DuoSecurity.Auth.Http*


2. Renamed DuoResponse.**Response** to DuoResponse.**Result** because I wanted to avoid this weird double "response" naming scenario:
```C#
var response = await client.PingAsync();
doSomething(response.Response); // no bueno
doSomething(response.Result); // bueno
```