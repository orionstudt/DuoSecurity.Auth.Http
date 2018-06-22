using DuoSecurity.Auth.Http.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.Results
{
    public class Device
    {
        /// <summary>
        /// The device is valid for automatic factor selection (e.g. phone or push).
        /// </summary>
        public bool CanAuto { get; }

        /// <summary>
        /// The device is activated for Duo Push.
        /// </summary>
        public bool CanPush { get; }

        /// <summary>
        /// The device can receive batches of SMS passcodes.
        /// </summary>
        public bool CanSMS { get; }

        /// <summary>
        /// The device can receive phone calls.
        /// </summary>
        public bool CanPhone { get; }

        /// <summary>
        /// The device is capable of generating passcodes with the Duo Mobile app.
        /// </summary>
        public bool CanMobileOTP { get; }

        /// <summary>
        /// List of strings, each a factor that can be used with the device.
        /// </summary>
        public IEnumerable<string> Capabilities { get; }

        /// <summary>
        /// Identifies which of the user's devices this is.
        /// </summary>
        public string DeviceId { get; }

        /// <summary>
        /// A short string which can be used to identify the device in a prompt.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Device's name. Or, if the device has not been named, the empty string ("").
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Single-character string containing the starting number of the next acceptable passcode previously SMSed to the user, if any.
        /// </summary>
        public string SMS_NextCode { get; }

        /// <summary>
        /// Phone number of the device. Or, if the device has no associated number, the empty string ("").
        /// </summary>
        public string PhoneNumber { get; }

        /// <summary>
        /// "phone" or "token".
        /// </summary>
        public DeviceType Type { get; }

        internal Device(DeviceModel model)
        {
            CanAuto = model.Capabilities.Any(c => c == "auto");
            CanPush = model.Capabilities.Any(c => c == "push");
            CanSMS = model.Capabilities.Any(c => c == "sms");
            CanPhone = model.Capabilities.Any(c => c == "phone");
            CanMobileOTP = model.Capabilities.Any(c => c == "mobile_otp");
            Capabilities = model.Capabilities.ToArray();
            DeviceId = model.Device;
            DisplayName = model.Display_Name;
            Name = model.Name;
            SMS_NextCode = model.Sms_Nextcode;
            PhoneNumber = model.Number;
            if (model.Type.ToLower() == "phone") Type = DeviceType.Phone;
            else Type = DeviceType.Token;
        }
    }

    public enum DeviceType
    {
        Phone,
        Token
    }
}
