using DuoSecurity.Auth.Http.Abstraction;
using DuoSecurity.Auth.Http.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.JsonModels
{
    public class AuthAsyncResultModel : IModel<AuthAsyncResult>
    {
        public string Txid { get; set; }

        public AuthAsyncResult ToResult()
        {
            return new AuthAsyncResult(this);
        }
    }
}
