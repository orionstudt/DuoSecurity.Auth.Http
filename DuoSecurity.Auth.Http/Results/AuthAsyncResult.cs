using System.Text.Json.Serialization;

namespace DuoSecurity.Auth.Http.Results;

public sealed record AuthAsyncResult
{
    /// <summary>
    /// A transaction ID to be used to query the authentication status using the auth status endpoint.
    /// </summary>
    [JsonPropertyName("txid")]
    public required string TransactionId { get; init; }
}