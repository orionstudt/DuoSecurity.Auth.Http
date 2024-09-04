using System.Text.Json.Serialization;
using DuoSecurity.Auth.Http.Internal;

namespace DuoSecurity.Auth.Http.Results;

[JsonConverter(typeof(UnderscoreSnakeCasedConverter))]
public enum DeviceCapability
{
    /// <summary>
    /// The device is valid for automatic factor selection (e.g. phone or push).
    /// </summary>
    Auto,
    /// <summary>
    /// The device is activated for Duo Push.
    /// </summary>
    Push,
    /// <summary>
    /// The device can receive batches of SMS passcodes.
    /// </summary>
    Sms,
    /// <summary>
    /// The device can receive phone calls.
    /// </summary>
    Phone,
    /// <summary>
    /// The device is capable of generating passcodes with the Duo Mobile app.
    /// </summary>
    MobileOtp
}