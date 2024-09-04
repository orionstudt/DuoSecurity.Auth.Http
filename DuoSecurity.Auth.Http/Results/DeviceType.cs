using System.Text.Json.Serialization;
using DuoSecurity.Auth.Http.Internal;

namespace DuoSecurity.Auth.Http.Results;

[JsonConverter(typeof(UnderscoreSnakeCasedConverter))]
public enum DeviceType
{
    Phone,
    Token
}