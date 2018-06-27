using DuoSecurity.Auth.Http.Abstraction;
using DuoSecurity.Auth.Http.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.JsonModels
{
    internal class EnrollResultModel : IJsonModel<EnrollResult>
    {
        public string Activation_Barcode { get; set; }

        public string Activation_Code { get; set; }

        public long Expiration { get; set; }

        public string User_Id { get; set; }

        public string Username { get; set; }

        public EnrollResult ToResult()
        {
            return new EnrollResult(this);
        }
    }
}
