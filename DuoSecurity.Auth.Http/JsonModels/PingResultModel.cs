using DuoSecurity.Auth.Http.Abstraction;
using DuoSecurity.Auth.Http.Results;

namespace DuoSecurity.Auth.Http.JsonModels
{
    internal class PingResultModel : IJsonModel<PingResult>
    {
        public long Time { get; set; }

        public PingResult ToResult()
        {
            return new PingResult(this);
        }
    }
}
