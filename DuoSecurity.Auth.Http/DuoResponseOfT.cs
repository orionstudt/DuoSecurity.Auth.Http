using System;
using System.Net.Http;
using DuoSecurity.Auth.Http.Results;

namespace DuoSecurity.Auth.Http;

/// <summary>
/// A response from the Duo Auth API.
/// </summary>
/// <typeparam name="T">The specific result type, depending on the endpoint.</typeparam>
public class DuoResponse<T> : IDisposable
{
    /// <summary>
    /// Indicates if the request returned a success status code.
    /// </summary>
    public bool IsSuccessful { get; internal set; }

    /// <summary>
    /// The de-serialized error response from the Duo Auth API.
    /// </summary>
    public DuoError? Error { get; internal set; }

    /// <summary>
    /// The original underlying response from the Duo Auth API.
    /// </summary>
    public HttpResponseMessage OriginalResponse { get; internal set; }

    /// <summary>
    /// The original JSON content of the response from the Duo Auth API.
    /// </summary>
    public string OriginalJson { get; internal set; }

    /// <summary>
    /// The specific result, which will depend on the endpoint.
    /// </summary>
    public T? Result { get; internal set; }

    internal DuoResponse() { }

    public void Dispose()
    {
        OriginalResponse?.Dispose();
        if (Result is IDisposable disposableResult)
            disposableResult.Dispose();
    }
}