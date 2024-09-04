using System.Collections.Generic;

namespace DuoSecurity.Auth.Http.Requests;

/// <summary>
/// Request parameters for /enroll endpoint.
/// </summary>
public class EnrollRequest : IParameterProvider
{
    /// <summary>
    /// Username for the created user. If not given, a random username will be assigned and returned.
    /// </summary>
    public string? UserName { get; set; }
    
    /// <summary>
    /// Seconds for which the activation code will remain valid. Default: 86400 (one day).
    /// </summary>
    public int? ValidSeconds { get; set; }

    public virtual IEnumerable<KeyValuePair<string, string>> GetParameters()
    {
        if (!string.IsNullOrWhiteSpace(UserName))
            yield return new KeyValuePair<string, string>("username", UserName);

        if (ValidSeconds.HasValue)
            yield return new KeyValuePair<string, string>("valid_secs", ValidSeconds.Value.ToString());
    }
}