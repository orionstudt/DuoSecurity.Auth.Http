namespace DuoSecurity.Auth.Http.JsonModels;

internal interface IJsonModel<T>
{
    T ToResult();
}