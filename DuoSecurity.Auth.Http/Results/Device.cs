using System.Collections.Frozen;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DuoSecurity.Auth.Http.Results;

public sealed record Device
{
    /// <summary>
    /// The device is valid for automatic factor selection (e.g. phone or push).
    /// </summary>
    public bool CanAuto => Capabilities.Contains(DeviceCapability.Auto);

    /// <summary>
    /// The device is activated for Duo Push.
    /// </summary>
    public bool CanPush => Capabilities.Contains(DeviceCapability.Push);

    /// <summary>
    /// The device can receive batches of SMS passcodes.
    /// </summary>
    public bool CanSms => Capabilities.Contains(DeviceCapability.Sms);

    /// <summary>
    /// The device can receive phone calls.
    /// </summary>
    public bool CanPhone => Capabilities.Contains(DeviceCapability.Phone);

    /// <summary>
    /// The device is capable of generating passcodes with the Duo Mobile app.
    /// </summary>
    public bool CanMobileOtp => Capabilities.Contains(DeviceCapability.MobileOtp);

    /// <summary>
    /// List of strings, each a factor that can be used with the device.
    /// </summary>
    [JsonPropertyName("capabilities")]
    public required HashSet<DeviceCapability> Capabilities { get; init; }

    /// <summary>
    /// Identifies which of the user's devices this is.
    /// </summary>
    [JsonPropertyName("device")]
    public required string DeviceId { get; init; }
    
    /// <summary>
    /// "phone" or "token".
    /// </summary>
    [JsonPropertyName("type")]
    public required DeviceType Type { get; init; }

    /// <summary>
    /// A short string which can be used to identify the device in a prompt.
    /// </summary>
    [JsonPropertyName("display_name")]
    public string? DisplayName { get; init; }

    /// <summary>
    /// Device's name. Or, if the device has not been named, the empty string ("").
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Single-character string containing the starting number of the next acceptable passcode
    /// previously SMSed to the user, if any.
    /// </summary>
    [JsonPropertyName("sms_nextcode")]
    public string? SmsNextCode { get; init; }

    /// <summary>
    /// Phone number of the device. Or, if the device has no associated number, the empty string ("").
    /// </summary>
    [JsonPropertyName("number")]
    public string? PhoneNumber { get; init; }
}