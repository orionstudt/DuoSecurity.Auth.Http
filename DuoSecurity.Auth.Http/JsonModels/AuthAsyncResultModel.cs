using DuoSecurity.Auth.Http.Results;

namespace DuoSecurity.Auth.Http.JsonModels;

public class AuthAsyncResultModel : IJsonModel<AuthAsyncResult>
{
    public string Txid { get; set; }

    public AuthAsyncResult ToResult()
        => new(this);
}