using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.Results
{
    public class EnrollStatusResult
    {
        /// <summary>
        /// Indicates whether a user has completed enrollment.
        /// </summary>
        public EnrollStatus Status { get; }

        internal EnrollStatusResult(string response)
        {
            switch (response.ToLower())
            {
                case "success":
                    Status = EnrollStatus.Success;
                    break;
                case "waiting":
                    Status = EnrollStatus.Waiting;
                    break;
                default:
                    Status = EnrollStatus.Invalid;
                    break;
            }
        }
    }

    public enum EnrollStatus
    {
        /// <summary>
        /// The user successfully added the account to Duo Mobile.
        /// </summary>
        Success,
        /// <summary>
        /// The code is expired or otherwise not valid for the specified user.
        /// </summary>
        Invalid,
        /// <summary>
        /// The code has not been claimed yet.
        /// </summary>
        Waiting
    }
}
