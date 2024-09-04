using System.Text.Json.Serialization;
using DuoSecurity.Auth.Http.Internal;

namespace DuoSecurity.Auth.Http.Results;

[JsonConverter(typeof(UnderscoreSnakeCasedConverter))]
public enum EnrollStatus
{
    /// <summary>
    /// The user successfully added the account to Duo Mobile.
    /// </summary>
    Success,
    /// <summary>
    /// The code is expired or otherwise not valid for the specified user.
    /// </summary>
    Invalid,
    /// <summary>
    /// The code has not been claimed yet.
    /// </summary>
    Waiting
}