using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DuoSecurity.Auth.Http.Internal;
using DuoSecurity.Auth.Http.Results;

namespace DuoSecurity.Auth.Http;

internal static class DuoResponse
{
    public static async Task<DuoResponse<TResult>> ParseAsync<TResult>(
        HttpResponseMessage response,
        CancellationToken cancel)
        where TResult : class
    {
        var content = await response.Content.ReadAsStringAsync(cancel);

        if (!response.IsSuccessStatusCode)
            return Error<TResult>(response, content);

        var wrapper = JsonSerializer.Deserialize<ResponseWrapper<TResult>>(content);
        return new DuoResponse<TResult>
        {
            IsSuccessful = true,
            Error = null,
            OriginalResponse = response,
            OriginalJson = content,
            Result = wrapper!.Response,
        };
    }

    public static async Task<DuoResponse<TResult>> ErrorAsync<TResult>(
        HttpResponseMessage response,
        CancellationToken cancel)
    {
        var content = await response.Content.ReadAsStringAsync(cancel);
        return Error<TResult>(response, content);
    }

    public static DuoResponse<TResult> Error<TResult>(HttpResponseMessage response, string content)
    {
        return new DuoResponse<TResult>
        {
            IsSuccessful = false,
            Error = JsonSerializer.Deserialize<DuoError>(content),
            OriginalResponse = response,
            OriginalJson = content,
            Result = default,
        };
    }
}