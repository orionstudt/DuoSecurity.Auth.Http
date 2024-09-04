using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace DuoSecurity.Auth.Http.Core;

internal class DuoRequestBuilder
{
    // Config
    private readonly string host;
    private readonly string integrationKey;
    private readonly string secretKey;

    private const string prefix = "auth/v2";

    public DuoRequestBuilder(DuoAuthConfig config)
    {
        host = config.Host;
        integrationKey = config.IntegrationKey;
        secretKey = config.SecretKey;
    }

    private HttpRequestMessage BuildMessage(HttpMethod method, string endpoint, params KeyValuePair<string, string>[] parameters)
    {
        // Url Encoded Parameters
        var urlParams = string.Empty;
        if (parameters.Any())
            urlParams = DuoApiHelper.CanonicalizeParams(parameters);

        // Date
        var dateStr = DuoApiHelper.DateToRFC822(DateTime.Now);

        // Init Message
        var mUpper = method.Method.ToUpper();
        HttpRequestMessage message;
        if ((mUpper == "GET" || mUpper == "DELETE") && !string.IsNullOrWhiteSpace(urlParams))
            message = new HttpRequestMessage(method, $"https://{host}/{prefix}/{endpoint}?{urlParams}");
        else message = new HttpRequestMessage(method, $"https://{host}/{prefix}/{endpoint}");

        // Headers
        message.Headers.Add("Accept", "application/json");
        message.Headers.Add("Host", host);
        message.Headers.Add("Date", dateStr);
        message.Headers.Add("X-Duo-Date", dateStr);

        // Signature
        var signature = DuoApiHelper.CanonicalizeRequest(
            mUpper,
            host.ToLower(),
            $"/{prefix}/{endpoint}",
            urlParams,
            dateStr);

        // Authorization
        var signed = DuoApiHelper.HmacSign(secretKey, signature);
        var auth = $"{integrationKey}:{signed}";
        message.Headers.Add("Authorization", $"Basic {DuoApiHelper.Encode64(auth)}");

        // Add Content Body if POST
        if (mUpper == "POST")
            message.Content = new FormUrlEncodedContent(parameters);

        return message;
    }

    public HttpRequestMessage PingRequest()
    {
        var message = new HttpRequestMessage(HttpMethod.Get, $"https://{host}/{prefix}/ping");
        message.Headers.Add("Date", DuoApiHelper.DateToRFC822(DateTime.Now));
        return message;
    }

    public HttpRequestMessage CheckRequest()
        => BuildMessage(HttpMethod.Get, "check");

    public HttpRequestMessage LogoRequest()
        => BuildMessage(HttpMethod.Get, "logo");

    public HttpRequestMessage EnrollRequest(string username, int? validSecs)
    {
        var parameters = new List<KeyValuePair<string, string>>();
        if (!string.IsNullOrWhiteSpace(username)) parameters.Add(new KeyValuePair<string, string>("username", username));
        if (validSecs.HasValue) parameters.Add(new KeyValuePair<string, string>("valid_secs", validSecs.Value.ToString()));
        return BuildMessage(HttpMethod.Post, "enroll", parameters.ToArray());
    }

    public HttpRequestMessage EnrollCheckRequest(string userId, string activationCode)
        => BuildMessage(
            HttpMethod.Post,
            "enroll_status",
            new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("user_id", userId),
                new KeyValuePair<string, string>("activation_code", activationCode),
            });

    public HttpRequestMessage PreAuthRequest(string userId, string username, string ipaddr, string trustedDeviceToken)
    {
        var parameters = new List<KeyValuePair<string, string>>();

        // User Parameter
        if (!string.IsNullOrWhiteSpace(userId))
            parameters.Add(new KeyValuePair<string, string>("user_id", userId));
        else if (!string.IsNullOrWhiteSpace(username))
            parameters.Add(new KeyValuePair<string, string>("username", username));

        // Optional Parameters
        if (!string.IsNullOrWhiteSpace(ipaddr))
            parameters.Add(new KeyValuePair<string, string>("ipaddr", ipaddr));        
            
        if (!string.IsNullOrWhiteSpace(trustedDeviceToken))
            parameters.Add(new KeyValuePair<string, string>("trusted_device_token", trustedDeviceToken));

        return BuildMessage(HttpMethod.Post, "preauth", parameters.ToArray());
    }

    public HttpRequestMessage AuthRequest(string userId, string username, string factor, string ipaddr, string async, string device, string passcode)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("factor", factor)
        };

        // User Parameter
        if (!string.IsNullOrWhiteSpace(userId))
            parameters.Add(new KeyValuePair<string, string>("user_id", userId));
        else if (!string.IsNullOrWhiteSpace(username))
            parameters.Add(new KeyValuePair<string, string>("username", username));

        // Factor Parameter
        if (!string.IsNullOrWhiteSpace(device))
            parameters.Add(new KeyValuePair<string, string>("device", device));
        else if (!string.IsNullOrWhiteSpace(passcode))
            parameters.Add(new KeyValuePair<string, string>("passcode", passcode));

        // Optional Parameters
        if (!string.IsNullOrWhiteSpace(ipaddr))
            parameters.Add(new KeyValuePair<string, string>("ipaddr", ipaddr));

        if (!string.IsNullOrWhiteSpace(async))
            parameters.Add(new KeyValuePair<string, string>("async", async));   
            
        return BuildMessage(HttpMethod.Post, "auth", parameters.ToArray());
    }

    public HttpRequestMessage AuthStatusRequest(string transactionId)
        => BuildMessage(
            HttpMethod.Get,
            "auth_status",
            new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("txid", transactionId),
            });
}