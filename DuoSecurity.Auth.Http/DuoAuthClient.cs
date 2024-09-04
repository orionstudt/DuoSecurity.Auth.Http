using DuoSecurity.Auth.Http.Results;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DuoSecurity.Auth.Http.Internal;
using DuoSecurity.Auth.Http.Requests;

namespace DuoSecurity.Auth.Http;

/// <summary>
/// HTTP Client for interacting with the DuoSecurity Auth API. Use <see cref="IDuoAuthClient"/> directly.
/// </summary>
public class DuoAuthClient : IDuoAuthClient, IDisposable
{
    // Request Builder
    private readonly DuoRequestBuilder _builder;

    // Client
    private readonly bool _ownsClient;
    private readonly HttpClient _client;

    public DuoAuthClient(DuoAuthConfig config)
        : this(config, true, new HttpClient())
    {
    }

    public DuoAuthClient(DuoAuthConfig config, HttpClient client)
        : this(config, false, client)
    {
    }

    private DuoAuthClient(DuoAuthConfig config, bool ownsClient, HttpClient client)
    {
        _builder = new DuoRequestBuilder(config);
        _ownsClient = ownsClient;
        _client = client;
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<PingResult>> PingAsync()
        => PingAsync(default);
    
    public async Task<DuoResponse<PingResult>> PingAsync(CancellationToken cancel)
    {
        var request = _builder.PingRequest();
        var response = await _client.SendAsync(request, cancel);
        return await DuoResponse.ParseAsync<PingResult>(response, cancel);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<PingResult>> CheckAsync()
        => CheckAsync(default);
    
    public async Task<DuoResponse<PingResult>> CheckAsync(CancellationToken cancel)
    {
        var request = _builder.CheckRequest();
        var response = await _client.SendAsync(request, cancel);
        return await DuoResponse.ParseAsync<PingResult>(response, cancel);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<LogoResult>> LogoAsync()
        => LogoAsync(default);
    
    public async Task<DuoResponse<LogoResult>> LogoAsync(CancellationToken cancel)
    {
        var request = _builder.LogoRequest();
        var response = await _client.SendAsync(request, cancel);

        if (!response.IsSuccessStatusCode)
            return await DuoResponse.ErrorAsync<LogoResult>(response, cancel);

        var stream = await response.Content.ReadAsStreamAsync(cancel);
        return new DuoResponse<LogoResult>
        {
            IsSuccessful = true,
            OriginalResponse = response,
            Result = new LogoResult
            {
                Stream = stream,
            },
        };
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<EnrollResult>> EnrollAsync(string? username = null, int? valid_secs = null,
        CancellationToken cancelToken = default)
    {
        return EnrollAsync(new EnrollRequest
        {
            UserName = username,
            ValidSeconds = valid_secs,
        }, cancelToken);
    }
    
    public async Task<DuoResponse<EnrollResult>> EnrollAsync(EnrollRequest request, CancellationToken cancel)
    {
        var req = _builder.EnrollRequest(request);
        var response = await _client.SendAsync(req, cancel);
        return await DuoResponse.ParseAsync<EnrollResult>(response, cancel);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<EnrollStatusResult>> EnrollStatusAsync(string user_id, string activation_code,
        CancellationToken cancelToken = default)
    {
        return EnrollStatusAsync(new EnrollStatusRequest
        {
            UserId = user_id,
            ActivationCode = activation_code,
        }, cancelToken);
    }

    public async Task<DuoResponse<EnrollStatusResult>> EnrollStatusAsync(EnrollStatusRequest request,
        CancellationToken cancel)
    {
        var req = _builder.EnrollStatusRequest(request);
        var response = await _client.SendAsync(req, cancel);
        var content = await response.Content.ReadAsStringAsync(cancel);

        if (!response.IsSuccessStatusCode)
            return DuoResponse.Error<EnrollStatusResult>(response, content);

        var model = JsonSerializer.Deserialize<ResponseWrapper<EnrollStatus>>(content);
        return new DuoResponse<EnrollStatusResult>
        {
            IsSuccessful = true,
            OriginalResponse = response,
            OriginalJson = content,
            Result = new EnrollStatusResult { Status = model!.Response },
        };
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<PreAuthResult>> PreAuthByUserIdAsync(string user_id, string? ipaddr = null,
        string? trusted_device_token = null, CancellationToken cancelToken = default)
    {
        return PreAuthAsync(new PreAuthRequest
        {
            UserIdentifier = UserIdentifier.UserId(user_id),
            IpAddress = ipaddr,
            TrustedDeviceToken = trusted_device_token,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<PreAuthResult>> PreAuthByUsernameAsync(string username, string? ipaddr = null,
        string? trusted_device_token = null, CancellationToken cancelToken = default)
    {
        return PreAuthAsync(new PreAuthRequest
        {
            UserIdentifier = UserIdentifier.UserName(username),
            IpAddress = ipaddr,
            TrustedDeviceToken = trusted_device_token,
        }, cancelToken);
    }

    public async Task<DuoResponse<PreAuthResult>> PreAuthAsync(PreAuthRequest request, CancellationToken cancel)
    {
        var req = _builder.PreAuthRequest(request);
        var response = await _client.SendAsync(req, cancel);
        return await DuoResponse.ParseAsync<PreAuthResult>(response, cancel);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthResult>> AuthAutoByUserIdAsync(string user_id, string? ipaddr = null,
        CancellationToken cancelToken = default)
    {
        return AuthAsync(new AutoAuthRequest
        {
            UserIdentifier = UserIdentifier.UserId(user_id),
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthResult>> AuthAutoByUsernameAsync(string username, string? ipaddr = null,
        CancellationToken cancelToken = default)
    {
        return AuthAsync(new AutoAuthRequest
        {
            UserIdentifier = UserIdentifier.UserName(username),
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthResult>> AuthPushByUserIdAsync(string user_id, string device = "auto",
        string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthAsync(new PushAuthRequest
        {
            UserIdentifier = UserIdentifier.UserId(user_id),
            DeviceId = device,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthResult>> AuthPushByUsernameAsync(string username, string device = "auto",
        string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthAsync(new PushAuthRequest
        {
            UserIdentifier = UserIdentifier.UserName(username),
            DeviceId = device,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthResult>> AuthPasscodeByUserIdAsync(string user_id, string passcode,
        string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthAsync(new PasscodeAuthRequest
        {
            UserIdentifier = UserIdentifier.UserId(user_id),
            Passcode = passcode,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthResult>> AuthPasscodeByUsernameAsync(string username, string passcode,
        string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthAsync(new PasscodeAuthRequest
        {
            UserIdentifier = UserIdentifier.UserName(username),
            Passcode = passcode,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthResult>> AuthSmsByUserIdAsync(string user_id, string device = "auto",
        string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthAsync(new PhoneAuthRequest
        {
            UserIdentifier = UserIdentifier.UserId(user_id),
            DeviceId = device,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthResult>> AuthSmsByUsernameAsync(string username, string device = "auto",
        string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthAsync(new PhoneAuthRequest
        {
            UserIdentifier = UserIdentifier.UserName(username),
            DeviceId = device,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthResult>> AuthPhoneByUserIdAsync(string user_id, string device = "auto",
        string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthAsync(new PhoneAuthRequest
        {
            UserIdentifier = UserIdentifier.UserId(user_id),
            DeviceId = device,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthResult>> AuthPhoneByUsernameAsync(string username,
        string device = "auto", string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthAsync(new PhoneAuthRequest
        {
            UserIdentifier = UserIdentifier.UserName(username),
            DeviceId = device,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthAsyncResult>> AuthAutoByUserIdForPollingAsync(string user_id,
        string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthWithPollingAsync(new AutoAuthRequest
        {
            UserIdentifier = UserIdentifier.UserId(user_id),
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthAsyncResult>> AuthAutoByUsernameForPollingAsync(string username,
        string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthWithPollingAsync(new AutoAuthRequest
        {
            UserIdentifier = UserIdentifier.UserName(username),
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthAsyncResult>> AuthPushByUserIdForPollingAsync(string user_id, string device = "auto",
        string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthWithPollingAsync(new PushAuthRequest
        {
            UserIdentifier = UserIdentifier.UserId(user_id),
            DeviceId = device,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthAsyncResult>> AuthPushByUsernameForPollingAsync(string username,
        string device = "auto", string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthWithPollingAsync(new PushAuthRequest
        {
            UserIdentifier = UserIdentifier.UserName(username),
            DeviceId = device,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthAsyncResult>> AuthPasscodeByUserIdForPollingAsync(string user_id, string passcode,
        string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthWithPollingAsync(new PasscodeAuthRequest
        {
            UserIdentifier = UserIdentifier.UserId(user_id),
            Passcode = passcode,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthAsyncResult>> AuthPasscodeByUsernameForPollingAsync(string username,
        string passcode, string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthWithPollingAsync(new PasscodeAuthRequest
        {
            UserIdentifier = UserIdentifier.UserName(username),
            Passcode = passcode,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthAsyncResult>> AuthSmsByUserIdForPollingAsync(string user_id,
        string device = "auto", string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthWithPollingAsync(new SmsAuthRequest
        {
            UserIdentifier = UserIdentifier.UserId(user_id),
            DeviceId = device,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthAsyncResult>> AuthSmsByUsernameForPollingAsync(string username,
        string device = "auto", string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthWithPollingAsync(new SmsAuthRequest
        {
            UserIdentifier = UserIdentifier.UserName(username),
            DeviceId = device,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthAsyncResult>> AuthPhoneByUserIdForPollingAsync(string user_id,
        string device = "auto", string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthWithPollingAsync(new PhoneAuthRequest
        {
            UserIdentifier = UserIdentifier.UserId(user_id),
            DeviceId = device,
            IpAddress = ipaddr,
        }, cancelToken);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthAsyncResult>> AuthPhoneByUsernameForPollingAsync(string username,
        string device = "auto", string? ipaddr = null, CancellationToken cancelToken = default)
    {
        return AuthWithPollingAsync(new PhoneAuthRequest
        {
            UserIdentifier = UserIdentifier.UserName(username),
            DeviceId = device,
            IpAddress = ipaddr,
        }, cancelToken);
    }
    
    public async Task<DuoResponse<AuthResult>> AuthAsync(AuthRequest request, CancellationToken cancel)
    {
        request.IsAsync = false;
        var req = _builder.AuthRequest(request);
        var response = await _client.SendAsync(req, cancel);
        return await DuoResponse.ParseAsync<AuthResult>(response, cancel);
    }
    
    public async Task<DuoResponse<AuthAsyncResult>> AuthWithPollingAsync(AuthRequest request, CancellationToken cancel)
    {
        request.IsAsync = true;
        var req = _builder.AuthRequest(request);
        var response = await _client.SendAsync(req, cancel);
        return await DuoResponse.ParseAsync<AuthAsyncResult>(response, cancel);
    }

    [Obsolete("Use the overload provided by the IDuoAuthClient interface.")]
    public Task<DuoResponse<AuthStatusResult>> AuthStatusAsync(string txid,
        CancellationToken cancelToken = default)
    {
        return AuthStatusAsync(new AuthStatusRequest { TransactionId = txid }, cancelToken);
    }

    public async Task<DuoResponse<AuthStatusResult>> AuthStatusAsync(AuthStatusRequest request,
        CancellationToken cancel)
    {
        var req = _builder.AuthStatusRequest(request);
        var response = await _client.SendAsync(req, cancel);
        return await DuoResponse.ParseAsync<AuthStatusResult>(response, cancel);
    }

    public void Dispose()
    {
        if (_ownsClient)
            _client?.Dispose();
    }
}