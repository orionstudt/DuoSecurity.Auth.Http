using DuoSecurity.Auth.Http.Abstraction;
using DuoSecurity.Auth.Http.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.JsonModels
{
    internal class PingResultModel : IModel<PingResult>
    {
        public long Time { get; set; }

        public PingResult ToResult()
        {
            return new PingResult(this);
        }
    }
}
