using System.Collections.Generic;
using System.Linq;

namespace DuoSecurity.Auth.Http.JsonModels
{
    internal class DeviceModel
    {
        private IEnumerable<string> _capabilities = Enumerable.Empty<string>();

        public IEnumerable<string> Capabilities
        {
            get => _capabilities;
            set => _capabilities = value ?? Enumerable.Empty<string>();
        }

        public string Device { get; set; }

        public string Display_Name { get; set; }

        public string Name { get; set; }

        public string Sms_Nextcode { get; set; }

        public string Number { get; set; }

        public string Type { get; set; }
    }
}
