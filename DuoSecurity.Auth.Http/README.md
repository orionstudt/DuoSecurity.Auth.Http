# DuoSecurity.Auth.Http

## Changes for 2.0.0 Release

1. Bumped to targeting `net8.0`
2. Removed dependency on `Newtonsoft.Json`
3. Removed some internal model->model mapping to cut down on allocations
4. The `DuoResponse` object is now properly `IDisposable` (it returns an `HttpResponseMessage`)
5. Provided a new `IDuoAuthClient` abstraction for DI or mocking
6. Continued to support existing APIs, but some new APIs were introduced that will make it easier to support new parameters as they are release without causing breaking changes. Marked the existing APIs as obsolete and they are not provided by the new abstraction, only by the concrete client.
7. Some design changes included with the new APIs is that the Request objects they take as parameters are extensible so that you may customize or add additional support for parameters manually without needing a patch from this library.