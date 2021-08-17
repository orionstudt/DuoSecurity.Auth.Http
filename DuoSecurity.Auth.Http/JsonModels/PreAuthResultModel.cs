using DuoSecurity.Auth.Http.Abstraction;
using DuoSecurity.Auth.Http.Results;
using System.Collections.Generic;

namespace DuoSecurity.Auth.Http.JsonModels
{
    internal class PreAuthResultModel : IJsonModel<PreAuthResult>
    {
        private ICollection<DeviceModel> _devices = new List<DeviceModel>();

        public string Result { get; set; }

        public string Status_Msg { get; set; }

        public ICollection<DeviceModel> Devices
        {
            get => _devices;
            set => _devices = value ?? new List<DeviceModel>();
        }

        public string Enroll_Portal_Url { get; set; }

        public PreAuthResult ToResult()
        {
            return new PreAuthResult(this);
        }
    }
}
