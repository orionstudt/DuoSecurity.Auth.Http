using DuoSecurity.Auth.Http.Abstraction;
using DuoSecurity.Auth.Http.Results;
using System.Collections.Generic;
using System.Linq;

namespace DuoSecurity.Auth.Http.JsonModels
{
    internal class PreAuthResultModel : IJsonModel<PreAuthResult>
    {
        private IEnumerable<DeviceModel> _devices = Enumerable.Empty<DeviceModel>();

        public string Result { get; set; }

        public string Status_Msg { get; set; }

        public IEnumerable<DeviceModel> Devices
        {
            get => _devices;
            set => _devices = value ?? Enumerable.Empty<DeviceModel>();
        }

        public string Enroll_Portal_Url { get; set; }

        public PreAuthResult ToResult()
        {
            return new PreAuthResult(this);
        }
    }
}
