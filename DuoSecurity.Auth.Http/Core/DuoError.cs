using DuoSecurity.Auth.Http.JsonModels;

namespace DuoSecurity.Auth.Http.Core
{
    public class DuoError
    {
        public int Code { get; }

        public string Message { get; }

        public string Detail { get; }

        internal DuoError(ErrorModel model)
        {
            Code = model.Code;
            Message = model.Message;
            Detail = model.Message_Detail;
        }
    }
}
