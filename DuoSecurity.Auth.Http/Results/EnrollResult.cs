using System;
using System.Text.Json.Serialization;

namespace DuoSecurity.Auth.Http.Results;

public sealed record EnrollResult
{
    /// <summary>
    /// URL for an image of a scannable barcode with the activation code.
    /// </summary>
    [JsonPropertyName("activation_barcode")]
    public required string ActivationBarCode { get; init; }

    /// <summary>
    /// Code to enter into the Duo Mobile app to add the account.
    /// On phones with Duo Mobile already installed it will be a clickable link.
    /// </summary>
    [JsonPropertyName("activation_code")]
    public required string ActivationCode { get; init; }
    
    /// <summary>
    /// Opening this URL on a phone with the Duo Mobile app installed will automatically complete activation.
    /// </summary>
    [JsonPropertyName("activation_url")]
    public required string ActivationUrl { get; init; }

    /// <summary>
    /// Time at which this activation code will expire, as a UNIX timestamp.
    /// </summary>
    [JsonPropertyName("expiration")]
    public required long Expiration { get; init; }

    /// <summary>
    /// Time at which this activation code will expire.
    /// </summary>
    public DateTime ExpirationUtc => DateTime.UnixEpoch.AddSeconds(Expiration);

    /// <summary>
    /// Permanent, unique identifier for the user in Duo.
    /// </summary>
    [JsonPropertyName("user_id")]
    public required string UserId { get; init; }

    /// <summary>
    /// Unique name for the user in Duo.
    /// </summary>
    [JsonPropertyName("username")]
    public required string UserName { get; init; }
}