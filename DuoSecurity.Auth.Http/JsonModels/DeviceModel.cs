using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.JsonModels
{
    internal class DeviceModel
    {
        public IEnumerable<string> Capabilities { get; set; }

        public string Device { get; set; }

        public string Display_Name { get; set; }

        public string Name { get; set; }

        public string Sms_Nextcode { get; set; }

        public string Number { get; set; }

        public string Type { get; set; }
    }
}
