﻿using DuoSecurity.Auth.Http.JsonModels;

namespace DuoSecurity.Auth.Http.Results;

public class AuthResult
{
    /// <summary>
    /// Indicates whether or not you should grant access to the user.
    /// </summary>
    public AuthState Result { get; }

    /// <summary>
    /// String detailing the progress or outcome of the authentication attempt. Use the result response to decide whether to grant access or not.
    /// </summary>
    public string Status { get; }

    /// <summary>
    /// A string describing the result of the authentication attempt. If the authentication attempt was denied, it may identify a reason. This string is intended for display to the user.
    /// </summary>
    public string StatusMessage { get; }

    /// <summary>
    /// A string containing a token for that trusted device, which can be passed into the /preauth endpoint. Requires the Remembered Devices option enabled in the Duo Admin Panel.
    /// </summary>
    public string TrustedDeviceToken { get; }

    internal AuthResult(AuthResultModel model)
    {
            Result = model.Result?.ToLower() == "allow" ? AuthState.Allow : AuthState.Deny;
            Status = model.Status;
            StatusMessage = model.Status_Msg;
            TrustedDeviceToken = model.Trusted_Device_Token;
        }
}