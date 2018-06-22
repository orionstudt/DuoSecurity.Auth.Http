using DuoSecurity.Auth.Http.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.Results
{
    public class AuthAsyncResult
    {
        public string TransactionId { get; }

        internal AuthAsyncResult(AuthAsyncResultModel model)
        {
            TransactionId = model.Txid;
        }
    }
}
