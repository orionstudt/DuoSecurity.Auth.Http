using System;
using System.Linq;
using System.Net.Http;
using DuoSecurity.Auth.Http.Requests;

namespace DuoSecurity.Auth.Http.Internal;

internal sealed class DuoRequestBuilder
{
    // Config
    private readonly string _host;
    private readonly string _integrationKey;
    private readonly string _secretKey;

    private const string Prefix = "auth/v2";

    public DuoRequestBuilder(DuoAuthConfig config)
    {
        _host = config.Host;
        _integrationKey = config.IntegrationKey;
        _secretKey = config.SecretKey;
    }

    private HttpRequestMessage BuildMessage(HttpMethod method, string endpoint, IParameterProvider? provider = null)
    {
        // Url Encoded Parameters
        var urlParams = string.Empty;
        var parameters = provider?.GetParameters().ToArray() ?? [];
        if (parameters.Any())
            urlParams = DuoApiHelper.CanonicalizeParams(parameters);

        // Date
        var dateStr = DuoApiHelper.DateToRFC822(DateTime.Now);

        // Init Message
        var mUpper = method.Method.ToUpper();
        HttpRequestMessage message;
        if ((mUpper == "GET" || mUpper == "DELETE") && !string.IsNullOrWhiteSpace(urlParams))
            message = new HttpRequestMessage(method, $"https://{_host}/{Prefix}/{endpoint}?{urlParams}");
        else message = new HttpRequestMessage(method, $"https://{_host}/{Prefix}/{endpoint}");

        // Headers
        message.Headers.Add("Accept", "application/json");
        message.Headers.Add("Host", _host);
        message.Headers.Add("Date", dateStr);
        message.Headers.Add("X-Duo-Date", dateStr);

        // Signature
        var signature = DuoApiHelper.CanonicalizeRequest(
            mUpper,
            _host.ToLower(),
            $"/{Prefix}/{endpoint}",
            urlParams,
            dateStr);

        // Authorization
        var signed = DuoApiHelper.HmacSign(_secretKey, signature);
        var auth = $"{_integrationKey}:{signed}";
        message.Headers.Add("Authorization", $"Basic {DuoApiHelper.Encode64(auth)}");

        // Add Content Body if POST
        if (mUpper == "POST")
            message.Content = new FormUrlEncodedContent(parameters);

        return message;
    }

    public HttpRequestMessage PingRequest()
    {
        var message = new HttpRequestMessage(HttpMethod.Get, $"https://{_host}/{Prefix}/ping");
        message.Headers.Add("Date", DuoApiHelper.DateToRFC822(DateTime.Now));
        return message;
    }

    public HttpRequestMessage CheckRequest()
        => BuildMessage(HttpMethod.Get, "check");

    public HttpRequestMessage LogoRequest()
        => BuildMessage(HttpMethod.Get, "logo");

    public HttpRequestMessage EnrollRequest(IParameterProvider provider)
        => BuildMessage(HttpMethod.Post, "enroll", provider);

    public HttpRequestMessage EnrollStatusRequest(IParameterProvider provider)
        => BuildMessage(HttpMethod.Post, "enroll_status", provider);
    
    public HttpRequestMessage PreAuthRequest(IParameterProvider provider)
        => BuildMessage(HttpMethod.Post, "preauth", provider);

    public HttpRequestMessage AuthRequest(IParameterProvider provider)
        => BuildMessage(HttpMethod.Post, "auth", provider);

    public HttpRequestMessage AuthStatusRequest(IParameterProvider provider)
        => BuildMessage(HttpMethod.Get, "auth_status", provider);
}