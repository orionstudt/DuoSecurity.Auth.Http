using System.Text.Json.Serialization;
using DuoSecurity.Auth.Http.Internal;

namespace DuoSecurity.Auth.Http.Results;

[JsonConverter(typeof(UnderscoreSnakeCasedConverter))]
public enum AuthStatus
{
    /// <summary>
    /// Currently calling the user's phone. The result will be "waiting".
    /// </summary>
    Calling,
    /// <summary>
    /// Phone call answered. The result will be "waiting".
    /// </summary>
    Answered,
    /// <summary>
    /// A Duo Push authentication request has been sent to the device. The result will be "waiting".
    /// </summary>
    Pushed,
    /// <summary>
    /// An error occurred while sending the push notification to the user's device. The user should retrieve the request manually using the Duo Push button in the Duo Mobile app. The result will be "waiting".
    /// </summary>
    PushFailed,
    /// <summary>
    /// Authentication timed out. Duo Push times out after 60 seconds and phone calls will also time out after approximately one minute. The result will be "waiting".
    /// </summary>
    Timeout,
    /// <summary>
    /// The authentication request was reported as fraudulent. The result will be "deny".
    /// </summary>
    Fraud,
    /// <summary>
    /// Authentication succeeded. The result will be "allow".
    /// </summary>
    Allow,
    /// <summary>
    /// Authentication has been skipped for a user in bypass mode. The result will be "allow".
    /// </summary>
    Bypass,
    /// <summary>
    /// Authentication denied. The result will be "deny".
    /// </summary>
    Deny,
    /// <summary>
    /// The user has been disabled due to authentication failures. The result will be "deny".
    /// </summary>
    LockedOut,
    /// <summary>
    /// Passcodes have been sent to the device. The result will be "deny".
    /// </summary>
    Sent
}