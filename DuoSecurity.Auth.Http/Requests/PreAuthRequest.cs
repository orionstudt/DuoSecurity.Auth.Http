using System.Collections.Generic;
using System.Security;

namespace DuoSecurity.Auth.Http.Requests;

/// <summary>
/// Request parameters for /preauth endpoint.
/// </summary>
public class PreAuthRequest : IParameterProvider
{
    /// <summary>
    /// The user identifier, either an ID or the username.
    /// </summary>
    public required UserIdentifier UserIdentifier { get; set; }
    
    /// <summary>
    /// Set to true if your client supports verified duo push.
    /// </summary>
    public bool? ClientSupportsVerifiedPush { get; set; }
    
    /// <summary>
    /// The IP address of the user to be authenticated, in dotted quad format. This will
    /// cause an "allow" response to be sent if appropriate for requests from a trusted network.
    /// </summary>
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// The host name of the device accessing the application.
    /// </summary>
    public string? HostName { get; set; }
    
    /// <summary>
    /// If the trusted device token is present and the application has a policy that remembers devices,
    /// this will cause an "allow" response to be sent for the lifetime of the token based on policy.
    /// </summary>
    public string? TrustedDeviceToken { get; set; }

    public IEnumerable<KeyValuePair<string, string>> GetParameters()
    {
        yield return UserIdentifier.GetParameter();
        
        if (ClientSupportsVerifiedPush is true)
            yield return new KeyValuePair<string, string>("client_supports_verified_push", "1");

        if (!string.IsNullOrWhiteSpace(IpAddress))
            yield return new KeyValuePair<string, string>("ipaddr", IpAddress);

        if (!string.IsNullOrWhiteSpace(HostName))
            yield return new KeyValuePair<string, string>("hostname", HostName);

        if (!string.IsNullOrWhiteSpace(TrustedDeviceToken))
            yield return new KeyValuePair<string, string>("trusted_device_token", TrustedDeviceToken);
    }
}