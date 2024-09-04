using System.Collections.Generic;

namespace DuoSecurity.Auth.Http.Requests;

/// <summary>
/// Request parameters for /enroll_status endpoint.
/// </summary>
public class EnrollStatusRequest : IParameterProvider
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    public required string UserId { get; set; }
    
    /// <summary>
    /// The activation code, as returned from the /enroll endpoint.
    /// </summary>
    public required string ActivationCode { get; set; }

    public virtual IEnumerable<KeyValuePair<string, string>> GetParameters()
    {
        yield return new KeyValuePair<string, string>("user_id", UserId);
        yield return new KeyValuePair<string, string>("activation_code", ActivationCode);
    }
}