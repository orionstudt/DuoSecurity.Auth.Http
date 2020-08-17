using DuoSecurity.Auth.Http.JsonModels;

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
