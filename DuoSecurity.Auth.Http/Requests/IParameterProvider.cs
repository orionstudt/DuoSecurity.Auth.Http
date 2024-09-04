using System.Collections.Generic;

namespace DuoSecurity.Auth.Http.Requests;

public interface IParameterProvider
{
    IEnumerable<KeyValuePair<string, string>> GetParameters();
}