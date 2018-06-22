using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.JsonModels
{
    internal class BaseModel
    {
        public string Stat { get; set; }
    }

    internal class BaseModel<T> : BaseModel where T : class
    {
        public T Response { get; set; }
    }
}
