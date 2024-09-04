using System.Text.Json.Serialization;

namespace DuoSecurity.Auth.Http.Results;

public sealed record AuthStatusResult
{
    /// <summary>
    /// Indicates the authentication attempt's current state.
    /// </summary>
    [JsonPropertyName("result")]
    public required AuthStatusState Result { get; init; }

    /// <summary>
    /// Indicates the progress or outcome of the authentication attempt.
    /// </summary>
    [JsonPropertyName("status")]
    public required AuthStatus Status { get; init; }

    /// <summary>
    /// Human-readable string describing the status of the authentication attempt. If the authentication attempt
    /// was denied, it may identify a reason. This string is intended for display to the user.
    /// </summary>
    [JsonPropertyName("status_msg")]
    public string? StatusMessage { get; init; }

    /// <summary>
    /// When /auth was called with async enabled, the value of trusted_device_token will be a string containing
    /// a token for that trusted device. It can be passed into the next /preauth call.
    /// Requires the Remembered Devices option enabled in the Duo Admin Panel.
    /// </summary>
    [JsonPropertyName("trusted_device_token")]
    public string? TrustedDeviceToken { get; init; }
}