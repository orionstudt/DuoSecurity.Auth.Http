using System;
using System.Text.Json.Serialization;

namespace DuoSecurity.Auth.Http.Results;

public sealed record PreAuthResult
{
    /// <summary>
    /// Indicates whether the user should be authenticated, bypassed, denied, or enrolled.
    /// </summary>
    [JsonPropertyName("result")]
    public required PreAuthState Result { get; init; }

    /// <summary>
    /// Human-readable message describing the result. This string is intended for display to the user.
    /// </summary>
    [JsonPropertyName("status_msg")]
    public string? StatusMessage { get; init; }
    
    /// <summary>
    /// UNIX timestamp of time at which the transaction ID and verification code for completing a verified
    /// push authentication expires, 60 seconds after issuance. Not present in the response when a regular
    /// push would be accepted.
    /// </summary>
    [JsonPropertyName("expiration")]
    public long? Expiration { get; init; }

    /// <summary>
    /// Time at which this activation code will expire.
    /// </summary>
    public DateTime? ExpirationUtc => Expiration is null ? null : DateTime.UnixEpoch.AddSeconds(Expiration.Value);
    
    /// <summary>
    /// If result is "enroll" a unique, enrollment portal URL is returned.
    /// This URL may be passed to the user and opened in a new browser window to access a portal that will help
    /// the user associate a device with the user_id specified or returned when calling /preauth.
    /// The enrollment URL is valid for five minutes after generation.
    /// </summary>
    [JsonPropertyName("enroll_portal_url")]
    public string? EnrollmentPortalUrl { get; init; }
    
    /// <summary>
    /// A transaction ID to submit to the /auth endpoint to perform a verified push.
    /// </summary>
    [JsonPropertyName("txid")]
    public string? TransactionId { get; init; }
    
    /// <summary>
    /// A numeric string of 3 to 6 digits matching the verified push code length configured in policy options.
    /// </summary>
    [JsonPropertyName("verification_code")]
    public string? VerificationCode { get; init; }

    /// <summary>
    /// An array of the user's devices.
    /// </summary>
    [JsonPropertyName("devices")]
    public required Device[] Devices { get; init; }
}