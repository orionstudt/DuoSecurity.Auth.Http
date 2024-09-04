using System.Text.Json.Serialization;
using DuoSecurity.Auth.Http.Internal;

namespace DuoSecurity.Auth.Http.Results;

[JsonConverter(typeof(UnderscoreSnakeCasedConverter))]
public enum AuthState
{
    /// <summary>
    /// Your application should grant access to the user.
    /// </summary>
    Allow,
    /// <summary>
    /// Your application should not grant access to the user.
    /// </summary>
    Deny
}