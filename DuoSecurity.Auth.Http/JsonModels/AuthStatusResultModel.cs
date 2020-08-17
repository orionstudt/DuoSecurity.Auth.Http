using DuoSecurity.Auth.Http.Abstraction;
using DuoSecurity.Auth.Http.Results;

namespace DuoSecurity.Auth.Http.JsonModels
{
    internal class AuthStatusResultModel : IJsonModel<AuthStatusResult>
    {
        public string Result { get; set; }

        public string Status { get; set; }

        public string Status_Msg { get; set; }

        public string Trusted_Device_Token { get; set; }

        public AuthStatusResult ToResult()
        {
            return new AuthStatusResult(this);
        }
    }
}
