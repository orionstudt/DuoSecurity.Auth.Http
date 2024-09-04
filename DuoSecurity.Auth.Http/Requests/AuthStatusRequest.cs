using System.Collections.Generic;

namespace DuoSecurity.Auth.Http.Requests;

/// <summary>
/// Request parameters for /auth_status endpoint.
/// </summary>
public class AuthStatusRequest : IParameterProvider
{
    /// <summary>
    /// The transaction ID of the authentication attempt, as returned by the /auth endpoint.
    /// </summary>
    public required string TransactionId { get; set; }

    public virtual IEnumerable<KeyValuePair<string, string>> GetParameters()
    {
        yield return new KeyValuePair<string, string>("txid", TransactionId);
    }
}