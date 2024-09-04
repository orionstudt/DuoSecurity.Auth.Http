using System.Text.Json.Serialization;

namespace DuoSecurity.Auth.Http.Results;

/// <summary>
/// A de-serialized error response from the Duo Auth API.
/// </summary>
public sealed record DuoError
{
    /// <summary>
    /// The duo-specific error code.
    /// </summary>
    [JsonPropertyName("code")]
    public required int Code { get; init; }

    /// <summary>
    /// A message describing the error.
    /// </summary>
    [JsonPropertyName("message")]
    public required string Message { get; init; }

    /// <summary>
    /// Additional information about the error, like the specific parameter that caused the error.
    /// </summary>
    [JsonPropertyName("message_detail")]
    public string? Detail { get; init; }
}