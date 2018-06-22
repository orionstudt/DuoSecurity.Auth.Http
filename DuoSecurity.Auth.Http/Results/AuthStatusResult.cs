using DuoSecurity.Auth.Http.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.Results
{
    public class AuthStatusResult
    {
        /// <summary>
        /// Indicates the authentication attempt's current state.
        /// </summary>
        public AuthStatusState Result { get; }

        /// <summary>
        /// Indicates the progress or outcome of the authentication attempt.
        /// </summary>
        public AuthStatus Status { get; }

        /// <summary>
        /// Human-readable string describing the status of the authentication attempt. If the authentication attempt was denied, it may identify a reason. This string is intended for display to the user.
        /// </summary>
        public string StatusMessage { get; }

        /// <summary>
        /// When /auth was called with async enabled, the value of trusted_device_token will be a string containing a token for that trusted device. It can be passed into the next /preauth call. Requires the Remembered Devices option enabled in the Duo Admin Panel.
        /// </summary>
        public string TrustedDeviceToken { get; }

        internal AuthStatusResult(AuthStatusResultModel model)
        {
            switch (model.Result.ToLower())
            {
                case "allow":
                    Result = AuthStatusState.Allow;
                    break;
                case "waiting":
                    Result = AuthStatusState.Waiting;
                    break;
                default:
                    Result = AuthStatusState.Deny;
                    break;
            }
            switch (model.Status.ToLower())
            {
                case "calling":
                    Status = AuthStatus.Calling;
                    break;
                case "answered":
                    Status = AuthStatus.Answered;
                    break;
                case "pushed":
                    Status = AuthStatus.Pushed;
                    break;
                case "push_failed":
                    Status = AuthStatus.PushFailed;
                    break;
                case "timeout":
                    Status = AuthStatus.Timeout;
                    break;
                case "fraud":
                    Status = AuthStatus.Fraud;
                    break;
                case "allow":
                    Status = AuthStatus.Allow;
                    break;
                case "bypass":
                    Status = AuthStatus.Bypass;
                    break;
                case "locked_out":
                    Status = AuthStatus.LockedOut;
                    break;
                case "sent":
                    Status = AuthStatus.Sent;
                    break;
                default:
                    Status = AuthStatus.Deny;
                    break;
            }
            StatusMessage = model.Status_Msg;
            TrustedDeviceToken = model.Trusted_Device_Token;
        }
    }

    public enum AuthStatusState
    {
        /// <summary>
        /// Authentication succeeded. Your application should grant access to the user.
        /// </summary>
        Allow,
        /// <summary>
        /// Authentication denied. Your application should deny access.
        /// </summary>
        Deny,
        /// <summary>
        /// Authentication is still in-progress. Your application should poll again until it finishes. Check the status for more details on the progress.
        /// </summary>
        Waiting
    }

    public enum AuthStatus
    {
        /// <summary>
        /// Currently calling the user's phone. The result will be "waiting".
        /// </summary>
        Calling,
        /// <summary>
        /// Phone call answered. The result will be "waiting".
        /// </summary>
        Answered,
        /// <summary>
        /// A Duo Push authentication request has been sent to the device. The result will be "waiting".
        /// </summary>
        Pushed,
        /// <summary>
        /// An error occurred while sending the push notification to the user's device. The user should retrieve the request manually using the Duo Push button in the Duo Mobile app. The result will be "waiting".
        /// </summary>
        PushFailed,
        /// <summary>
        /// Authentication timed out. Duo Push times out after 60 seconds and phone calls will also time out after approximately one minute. The result will be "waiting".
        /// </summary>
        Timeout,
        /// <summary>
        /// The authentication request was reported as fraudulent. The result will be "deny".
        /// </summary>
        Fraud,
        /// <summary>
        /// Authentication succeeded. The result will be "allow".
        /// </summary>
        Allow,
        /// <summary>
        /// Authentication has been skipped for a user in bypass mode. The result will be "allow".
        /// </summary>
        Bypass,
        /// <summary>
        /// Authentication denied. The result will be "deny".
        /// </summary>
        Deny,
        /// <summary>
        /// The user has been disabled due to authentication failures. The result will be "deny".
        /// </summary>
        LockedOut,
        /// <summary>
        /// Passcodes have been sent to the device. The result will be "deny".
        /// </summary>
        Sent
    }
}
