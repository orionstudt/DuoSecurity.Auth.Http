using System.Text.Json.Serialization;

namespace DuoSecurity.Auth.Http.Internal;

internal sealed record ResponseWrapper<T>
{
    [JsonPropertyName("response")]
    public required T Response { get; init; }
}