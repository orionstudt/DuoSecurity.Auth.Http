using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.JsonModels
{
    internal class ErrorModel : BaseModel
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Message_Detail { get; set; }
    }
}
