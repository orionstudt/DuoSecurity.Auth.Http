using DuoSecurity.Auth.Http.JsonModels;
using System.Collections.Generic;
using System.Linq;

namespace DuoSecurity.Auth.Http.Results;

public class PreAuthResult
{
    /// <summary>
    /// Indicates whether the user should be authenticated, bypassed, denied, or enrolled.
    /// </summary>
    public PreAuthState Result { get; }

    /// <summary>
    /// Human-readable message describing the result. This string is intended for display to the user.
    /// </summary>
    public string StatusMessage { get; }

    /// <summary>
    /// A list of the user's devices.
    /// </summary>
    public IEnumerable<Device> Devices { get; }

    /// <summary>
    /// If result is "enroll" a unique, enrollment portal URL is returned. This URL may be passed to the user and opened in a new browser window to access a portal that will help the user associate a device with the user_id specified or returned when calling /preauth. The enrollment URL is valid for five minutes after generation.
    /// </summary>
    public string EnrollmentPortalUrl { get; }

    internal PreAuthResult(PreAuthResultModel model)
    {
            switch (model.Result?.ToLower())
            {
                case "auth":
                    Result = PreAuthState.Auth;
                    break;
                case "allow":
                    Result = PreAuthState.Allow;
                    break;
                case "enroll":
                    Result = PreAuthState.Enroll;
                    break;
                default:
                    Result = PreAuthState.Deny;
                    break;
            }

            StatusMessage = model.Status_Msg;
            Devices = model.Devices?.Select(d => new Device(d)).ToList() ?? Enumerable.Empty<Device>();
            EnrollmentPortalUrl = model.Enroll_Portal_Url;
        }
}