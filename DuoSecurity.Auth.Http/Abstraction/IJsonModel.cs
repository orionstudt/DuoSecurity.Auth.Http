using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.Abstraction
{
    internal interface IJsonModel<T>
    {
        T ToResult();
    }
}
