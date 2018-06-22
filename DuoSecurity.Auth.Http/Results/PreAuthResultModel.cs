using DuoSecurity.Auth.Http.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.Results
{
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
            switch (model.Result.ToLower())
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
            var devices = new List<Device>();
            if (model.Devices != null && model.Devices.Any()) foreach (var d in model.Devices) devices.Add(new Device(d));
            Devices = devices;
            EnrollmentPortalUrl = model.Enroll_Portal_Url;
        }
    }

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
}
