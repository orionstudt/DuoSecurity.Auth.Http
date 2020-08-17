using DuoSecurity.Auth.Http.JsonModels;

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
}
