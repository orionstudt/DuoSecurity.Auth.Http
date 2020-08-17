using DuoSecurity.Auth.Http.JsonModels;
using System;

namespace DuoSecurity.Auth.Http.Results
{
    public class EnrollResult
    {
        /// <summary>
        /// URL for an image of a scannable barcode with the activation code.
        /// </summary>
        public string ActivationBarCode { get; }

        /// <summary>
        /// Code to enter into the Duo Mobile app to add the account. On phones with Duo Mobile already installed it will be a clickable link.
        /// </summary>
        public string ActivationCode { get; }

        /// <summary>
        /// Time at which this activation code will expire.
        /// </summary>
        public DateTime ExpirationUtc { get; }

        /// <summary>
        /// Permanent, unique identifier for the user in Duo.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Unique name for the user in Duo.
        /// </summary>
        public string UserName { get; }

        internal EnrollResult(EnrollResultModel model)
        {
            ActivationBarCode = model.Activation_Barcode;
            ActivationCode = model.Activation_Code;
            ExpirationUtc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(model.Expiration);
            UserId = model.User_Id;
            UserName = model.Username;
        }
    }
}
