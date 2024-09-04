using System.Collections.Generic;

namespace DuoSecurity.Auth.Http.Requests;

/// <summary>
/// Base request parameters for the /auth endpoint. Does not include factor or factor-specific parameters.
/// </summary>
public abstract class AuthRequest : IParameterProvider
{
    /// <summary>
    /// The user identifier, either an ID or the username.
    /// </summary>
    public required UserIdentifier UserIdentifier { get; set; }
    
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
    /// If true, will enable the async authentication flow which will require the /auth_status endpoint.
    /// </summary>
    internal bool? IsAsync { get; set; }
    
    /// <summary>
    /// The authentication factor that should be used for the auth request.
    /// </summary>
    protected abstract string Factor { get; }
    
    public virtual IEnumerable<KeyValuePair<string, string>> GetParameters()
    {
        yield return UserIdentifier.GetParameter();

        if (!string.IsNullOrWhiteSpace(IpAddress))
            yield return new KeyValuePair<string, string>("ipaddr", IpAddress);

        if (!string.IsNullOrWhiteSpace(HostName))
            yield return new KeyValuePair<string, string>("hostname", HostName);

        if (IsAsync is true)
            yield return new KeyValuePair<string, string>("async", "1");

        yield return new KeyValuePair<string, string>("factor", Factor);
        foreach (var parameter in GetFactorParameters())
            yield return parameter;
    }

    protected virtual IEnumerable<KeyValuePair<string, string>> GetFactorParameters()
    {
        yield break;
    }
}

/// <summary>
/// Auto factor auth request parameters for the /auth endpoint.
/// </summary>
public class AutoAuthRequest : AuthRequest
{
    protected override string Factor => "auto";
    
    protected override IEnumerable<KeyValuePair<string, string>> GetFactorParameters()
    {
        yield return new KeyValuePair<string, string>("device", "auto");
    }
}

/// <summary>
/// Push factor auth request parameters for the /auth endpoint.
/// </summary>
public class PushAuthRequest : AuthRequest
{
    protected override string Factor => "push";
    
    /// <summary>
    /// The ID of the device to push to. Must have push capacity.
    /// </summary>
    public required string DeviceId { get; set; }
    
    /// <summary>
    /// Can be used to customize the Duo Push authentication request as it is displayed.
    /// </summary>
    public string? Type { get; set; }
    
    /// <summary>
    /// Will be shown in place of the user's username.
    /// </summary>
    public string? DisplayUserName { get; set; }
    
    /// <summary>
    /// Can be used to add contextual information to the authentication request. Should be a URL
    /// encoded set of key/value pairs.
    /// </summary>
    public string? PushInfo { get; set; }
    
    /// <summary>
    /// The transaction ID received from the /preauth endpoint response when verified push wa specified.
    /// </summary>
    public string? TransactionId { get; set; }
    
    protected override IEnumerable<KeyValuePair<string, string>> GetFactorParameters()
    {
        yield return new KeyValuePair<string, string>("device", DeviceId);

        if (!string.IsNullOrWhiteSpace(Type))
            yield return new KeyValuePair<string, string>("type", Type);

        if (!string.IsNullOrWhiteSpace(DisplayUserName))
            yield return new KeyValuePair<string, string>("display_username", DisplayUserName);

        if (!string.IsNullOrWhiteSpace(PushInfo))
            yield return new KeyValuePair<string, string>("pushinfo", PushInfo);

        if (!string.IsNullOrWhiteSpace(TransactionId))
            yield return new KeyValuePair<string, string>("txid", TransactionId);
    }
}

/// <summary>
/// Passcode factor auth request parameters for the /auth endpoint.
/// </summary>
public class PasscodeAuthRequest : AuthRequest
{
    protected override string Factor => "passcode";
    
    /// <summary>
    /// Passcode entered by the user.
    /// </summary>
    public required string Passcode { get; set; }
    
    protected override IEnumerable<KeyValuePair<string, string>> GetFactorParameters()
    {
        yield return new KeyValuePair<string, string>("passcode", Passcode);
    }
}

/// <summary>
/// Phone factor auth request parameters for the /auth endpoint.
/// </summary>
public class PhoneAuthRequest : AuthRequest
{
    protected override string Factor => "phone";
    
    /// <summary>
    /// The ID of the device to push to. Must have phone capacity.
    /// </summary>
    public required string DeviceId { get; set; }
    
    protected override IEnumerable<KeyValuePair<string, string>> GetFactorParameters()
    {
        yield return new KeyValuePair<string, string>("device", DeviceId);
    }
}

/// <summary>
/// SMS factor auth request parameters for the /auth endpoint.
/// </summary>
public class SmsAuthRequest : AuthRequest
{
    protected override string Factor => "sms";
    
    /// <summary>
    /// The ID of the device to push to. Must have phone capacity.
    /// </summary>
    public required string DeviceId { get; set; }
    
    protected override IEnumerable<KeyValuePair<string, string>> GetFactorParameters()
    {
        yield return new KeyValuePair<string, string>("device", DeviceId);
    }
}