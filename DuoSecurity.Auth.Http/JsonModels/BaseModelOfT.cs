namespace DuoSecurity.Auth.Http.JsonModels;

internal class BaseModel<T> : BaseModel
    where T : class
{
    public T Response { get; set; }
}