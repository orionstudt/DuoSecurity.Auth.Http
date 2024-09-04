using System.Text.Json;
using System.Text.Json.Serialization;

namespace DuoSecurity.Auth.Http.Internal;

internal sealed class UnderscoreSnakeCasedConverter : JsonStringEnumConverter
{
    public UnderscoreSnakeCasedConverter()
        : base(JsonNamingPolicy.SnakeCaseLower, allowIntegerValues: true)
    {
    }
}