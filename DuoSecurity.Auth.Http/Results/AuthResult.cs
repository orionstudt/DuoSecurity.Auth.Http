using System.Text.Json.Serialization;

namespace DuoSecurity.Auth.Http.Results;

public sealed record AuthResult
{
    /// <summary>
    /// Indicates whether you should grant access to the user.
    /// </summary>
    [JsonPropertyName("result")]
    public required AuthState Result { get; init; }

    /// <summary>
    /// String detailing the progress or outcome of the authentication attempt.
    /// Use the result response to decide whether to grant access or not.
    /// </summary>
    [JsonPropertyName("status")]
    public required AuthStatus Status { get; init; }

    /// <summary>
    /// A string describing the result of the authentication attempt. If the authentication attempt was denied,
    /// it may identify a reason. This string is intended for display to the user.
    /// </summary>
    [JsonPropertyName("status_msg")]
    public string? StatusMessage { get; init; }

    /// <summary>
    /// A string containing a token for that trusted device, which can be passed into the /preauth endpoint.
    /// Requires the Remembered Devices option enabled in the Duo Admin Panel.
    /// </summary>
    [JsonPropertyName("trusted_device_token")]
    public string? TrustedDeviceToken { get; init; }
}