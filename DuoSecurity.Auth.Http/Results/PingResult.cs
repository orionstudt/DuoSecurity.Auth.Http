using System;
using System.Text.Json.Serialization;

namespace DuoSecurity.Auth.Http.Results;

public sealed record PingResult
{
    /// <summary>
    /// Current server time, as a UNIX timestamp.
    /// </summary>
    [JsonPropertyName("time")]
    public required long Time { get; init; }
    
    /// <summary>
    /// Current server time.
    /// </summary>
    public DateTime TimeUtc => DateTime.UnixEpoch.AddSeconds(Time);
}