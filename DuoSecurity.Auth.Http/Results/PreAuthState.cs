using System.Text.Json.Serialization;
using DuoSecurity.Auth.Http.Internal;

namespace DuoSecurity.Auth.Http.Results;

[JsonConverter(typeof(UnderscoreSnakeCasedConverter))]
public enum PreAuthState
{
    /// <summary>
    /// The user is known and permitted to authenticate. Your client application should use the /auth endpoint to perform authentication.
    /// </summary>
    Auth,
    /// <summary>
    /// The user is configured to bypass secondary authentication. Your client application should immediately grant access.
    /// </summary>
    Allow,
    /// <summary>
    /// The user is not permitted to authenticate at this time. Your client application should immediately deny access.
    /// </summary>
    Deny,
    /// <summary>
    /// The user is not known to Duo and needs to enroll. Your application should deny access.
    /// </summary>
    Enroll
}