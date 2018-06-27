using DuoSecurity.Auth.Http.Abstraction;
using DuoSecurity.Auth.Http.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.JsonModels
{
    internal class PreAuthResultModel : IJsonModel<PreAuthResult>
    {
        public string Result { get; set; }

        public string Status_Msg { get; set; }

        public IEnumerable<DeviceModel> Devices { get; set; }

        public string Enroll_Portal_Url { get; set; }

        public PreAuthResult ToResult()
        {
            return new PreAuthResult(this);
        }
    }
}
