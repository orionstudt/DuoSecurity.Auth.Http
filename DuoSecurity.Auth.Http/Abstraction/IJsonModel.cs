namespace DuoSecurity.Auth.Http.Abstraction
{
    internal interface IJsonModel<T>
    {
        T ToResult();
    }
}
