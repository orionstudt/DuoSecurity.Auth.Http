using DuoSecurity.Auth.Http.Core;
using DuoSecurity.Auth.Http.JsonModels;
using DuoSecurity.Auth.Http.Results;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http
{
    public class DuoAuthClient : IDisposable
    {
        // Request Builder
        private readonly DuoRequestBuilder builder;

        // Client
        private readonly bool ownsClient;
        private readonly HttpClient client;

        public DuoAuthClient(DuoAuthConfig config, HttpClient client = null)
        {
            this.builder = new DuoRequestBuilder(config);
            if (client == null)
            {
                this.ownsClient = true;
                this.client = new HttpClient();
            }
            else this.client = client;
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/ping
        /// </summary>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<PingResult>> PingAsync(CancellationToken cancelToken = default)
        {
            var request = builder.PingRequest();
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<PingResultModel, PingResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/check
        /// </summary>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<PingResult>> CheckAsync(CancellationToken cancelToken = default)
        {
            var request = builder.CheckRequest();
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<PingResultModel, PingResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/logo
        /// </summary>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<LogoResult>> LogoAsync(CancellationToken cancelToken = default)
        {
            var request = builder.LogoRequest();
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return await DuoResponse.ErrorAsync<LogoResult>(response).ConfigureAwait(false);

            var content = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            return new DuoResponse<LogoResult>
            {
                IsSuccessful = true,
                OriginalResponse = response,
                Result = new LogoResult(content)
            };
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/enroll
        /// </summary>
        /// <param name="username">Username for the created user. If not given, a random username will be assigned and returned.</param>
        /// <param name="valid_secs">Seconds for which the activation code will remain valid. Default: 86400 (one day).</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<EnrollResult>> EnrollAsync(string username = null, int? valid_secs = null, CancellationToken cancelToken = default)
        {
            var request = builder.EnrollRequest(username, valid_secs);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<EnrollResultModel, EnrollResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/enroll_status
        /// </summary>
        /// <param name="user_id">ID of the user.</param>
        /// <param name="activation_code">Activation code, as returned from /enroll.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<EnrollStatusResult>> EnrollStatusAsync(string user_id, string activation_code, CancellationToken cancelToken = default)
        {
            var request = builder.EnrollCheckRequest(user_id, activation_code);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return DuoResponse.Error<EnrollStatusResult>(response, content);

            var model = JsonConvert.DeserializeObject<BaseModel<string>>(content);
            return new DuoResponse<EnrollStatusResult>
            {
                IsSuccessful = true,
                OriginalResponse = response,
                OriginalJson = content,
                Result = new EnrollStatusResult(model.Response)
            };
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/preauth
        /// </summary>
        /// <param name="user_id">Permanent, unique identifier for the user as generated by Duo upon user creation (e.g. DUYHV6TJBC3O4RITS1WC). Exactly one of user_id and username must be specified.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="trusted_device_token">If the trusted_device_token is present and the Remembered Devices option is enabled in the Duo Admin Panel, return an "allow" response for the period of time a device may be remembered as set by the Duo administrator.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<PreAuthResult>> PreAuthByUserIdAsync(string user_id, string ipaddr = null, string trusted_device_token = null, CancellationToken cancelToken = default)
        {
            var request = builder.PreAuthRequest(user_id, null, ipaddr, trusted_device_token);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<PreAuthResultModel, PreAuthResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/preauth
        /// </summary>
        /// <param name="username">Unique identifier for the user that is commonly specified by your application during user creation (e.g. user@domain.com). Exactly one of user_id and username must be specified.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="trusted_device_token">If the trusted_device_token is present and the Remembered Devices option is enabled in the Duo Admin Panel, return an "allow" response for the period of time a device may be remembered as set by the Duo administrator.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<PreAuthResult>> PreAuthByUsernameAsync(string username, string ipaddr = null, string trusted_device_token = null, CancellationToken cancelToken = default)
        {
            var request = builder.PreAuthRequest(null, username, ipaddr, trusted_device_token);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<PreAuthResultModel, PreAuthResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="user_id">Permanent, unique identifier for the user as generated by Duo upon user creation (e.g. DUYHV6TJBC3O4RITS1WC). Exactly one of user_id and username must be specified.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthResult>> AuthAutoByUserIdAsync(string user_id, string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(user_id, null, "auto", ipaddr, null, "auto", null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthResultModel, AuthResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="username">Unique identifier for the user that is commonly specified by your application during user creation (e.g. user@domain.com). Exactly one of user_id and username must be specified.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthResult>> AuthAutoByUsernameAsync(string username, string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(null, username, "auto", ipaddr, null, "auto", null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthResultModel, AuthResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="user_id">Permanent, unique identifier for the user as generated by Duo upon user creation (e.g. DUYHV6TJBC3O4RITS1WC). Exactly one of user_id and username must be specified.</param>
        /// <param name="device">ID of the device. This device must have the "push" capability. You may also specify "auto" to use the first of the user's devices with the "push" capability.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthResult>> AuthPushByUserIdAsync(string user_id, string device = "auto", string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(user_id, null, "push", ipaddr, null, device, null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthResultModel, AuthResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="username">Unique identifier for the user that is commonly specified by your application during user creation (e.g. user@domain.com). Exactly one of user_id and username must be specified.</param>
        /// <param name="device">ID of the device. This device must have the "push" capability. You may also specify "auto" to use the first of the user's devices with the "push" capability.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthResult>> AuthPushByUsernameAsync(string username, string device = "auto", string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(null, username, "push", ipaddr, null, device, null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthResultModel, AuthResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="user_id">Permanent, unique identifier for the user as generated by Duo upon user creation (e.g. DUYHV6TJBC3O4RITS1WC). Exactly one of user_id and username must be specified.</param>
        /// <param name="passcode">Passcode entered by the user.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthResult>> AuthPasscodeByUserIdAsync(string user_id, string passcode, string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(user_id, null, "passcode", ipaddr, null, null, passcode);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthResultModel, AuthResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="username">Unique identifier for the user that is commonly specified by your application during user creation (e.g. user@domain.com). Exactly one of user_id and username must be specified.</param>
        /// <param name="passcode">Passcode entered by the user.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthResult>> AuthPasscodeByUsernameAsync(string username, string passcode, string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(null, username, "passcode", ipaddr, null, null, passcode);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthResultModel, AuthResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="user_id">Permanent, unique identifier for the user as generated by Duo upon user creation (e.g. DUYHV6TJBC3O4RITS1WC). Exactly one of user_id and username must be specified.</param>
        /// <param name="device">ID of the device to send passcodes to. This device must have the "sms" capability. You may also specify "auto" to use the first of the user's devices with the "sms" capability.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthResult>> AuthSmsByUserIdAsync(string user_id, string device = "auto", string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(user_id, null, "sms", ipaddr, null, device, null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthResultModel, AuthResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="username">Unique identifier for the user that is commonly specified by your application during user creation (e.g. user@domain.com). Exactly one of user_id and username must be specified.</param>
        /// <param name="device">ID of the device to send passcodes to. This device must have the "sms" capability. You may also specify "auto" to use the first of the user's devices with the "sms" capability.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthResult>> AuthSmsByUsernameAsync(string username, string device = "auto", string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(null, username, "sms", ipaddr, null, device, null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthResultModel, AuthResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="user_id">Permanent, unique identifier for the user as generated by Duo upon user creation (e.g. DUYHV6TJBC3O4RITS1WC). Exactly one of user_id and username must be specified.</param>
        /// <param name="device">ID of the device to call. This device must have the "phone" capability. You may also specify "auto" to use the first of the user's devices with the "phone" capability.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthResult>> AuthPhoneByUserIdAsync(string user_id, string device = "auto", string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(user_id, null, "phone", ipaddr, null, device, null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthResultModel, AuthResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="username">Unique identifier for the user that is commonly specified by your application during user creation (e.g. user@domain.com). Exactly one of user_id and username must be specified.</param>
        /// <param name="device">ID of the device to call. This device must have the "phone" capability. You may also specify "auto" to use the first of the user's devices with the "phone" capability.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthResult>> AuthPhoneByUsernameAsync(string username, string device = "auto", string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(null, username, "phone", ipaddr, null, device, null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthResultModel, AuthResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="user_id">Permanent, unique identifier for the user as generated by Duo upon user creation (e.g. DUYHV6TJBC3O4RITS1WC). Exactly one of user_id and username must be specified.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthAsyncResult>> AuthAutoByUserIdForPollingAsync(string user_id, string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(user_id, null, "auto", ipaddr, "1", "auto", null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthAsyncResultModel, AuthAsyncResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="username">Unique identifier for the user that is commonly specified by your application during user creation (e.g. user@domain.com). Exactly one of user_id and username must be specified.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthAsyncResult>> AuthAutoByUsernameForPollingAsync(string username, string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(null, username, "auto", ipaddr, "1", "auto", null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthAsyncResultModel, AuthAsyncResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="user_id">Permanent, unique identifier for the user as generated by Duo upon user creation (e.g. DUYHV6TJBC3O4RITS1WC). Exactly one of user_id and username must be specified.</param>
        /// <param name="device">ID of the device. This device must have the "push" capability. You may also specify "auto" to use the first of the user's devices with the "push" capability.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthAsyncResult>> AuthPushByUserIdForPollingAsync(string user_id, string device = "auto", string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(user_id, null, "push", ipaddr, "1", device, null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthAsyncResultModel, AuthAsyncResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="username">Unique identifier for the user that is commonly specified by your application during user creation (e.g. user@domain.com). Exactly one of user_id and username must be specified.</param>
        /// <param name="device">ID of the device. This device must have the "push" capability. You may also specify "auto" to use the first of the user's devices with the "push" capability.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthAsyncResult>> AuthPushByUsernameForPollingAsync(string username, string device = "auto", string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(null, username, "push", ipaddr, "1", device, null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthAsyncResultModel, AuthAsyncResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="user_id">Permanent, unique identifier for the user as generated by Duo upon user creation (e.g. DUYHV6TJBC3O4RITS1WC). Exactly one of user_id and username must be specified.</param>
        /// <param name="passcode">Passcode entered by the user.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthAsyncResult>> AuthPasscodeByUserIdForPollingAsync(string user_id, string passcode, string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(user_id, null, "passcode", ipaddr, "1", null, passcode);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthAsyncResultModel, AuthAsyncResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="username">Unique identifier for the user that is commonly specified by your application during user creation (e.g. user@domain.com). Exactly one of user_id and username must be specified.</param>
        /// <param name="passcode">Passcode entered by the user.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthAsyncResult>> AuthPasscodeByUsernameForPollingAsync(string username, string passcode, string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(null, username, "passcode", ipaddr, "1", null, passcode);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthAsyncResultModel, AuthAsyncResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="user_id">Permanent, unique identifier for the user as generated by Duo upon user creation (e.g. DUYHV6TJBC3O4RITS1WC). Exactly one of user_id and username must be specified.</param>
        /// <param name="device">ID of the device to send passcodes to. This device must have the "sms" capability. You may also specify "auto" to use the first of the user's devices with the "sms" capability.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthAsyncResult>> AuthSmsByUserIdForPollingAsync(string user_id, string device = "auto", string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(user_id, null, "sms", ipaddr, "1", device, null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthAsyncResultModel, AuthAsyncResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="username">Unique identifier for the user that is commonly specified by your application during user creation (e.g. user@domain.com). Exactly one of user_id and username must be specified.</param>
        /// <param name="device">ID of the device to send passcodes to. This device must have the "sms" capability. You may also specify "auto" to use the first of the user's devices with the "sms" capability.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthAsyncResult>> AuthSmsByUsernameForPollingAsync(string username, string device = "auto", string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(null, username, "sms", ipaddr, "1", device, null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthAsyncResultModel, AuthAsyncResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="user_id">Permanent, unique identifier for the user as generated by Duo upon user creation (e.g. DUYHV6TJBC3O4RITS1WC). Exactly one of user_id and username must be specified.</param>
        /// <param name="device">ID of the device to call. This device must have the "phone" capability. You may also specify "auto" to use the first of the user's devices with the "phone" capability.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthAsyncResult>> AuthPhoneByUserIdForPollingAsync(string user_id, string device = "auto", string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(user_id, null, "phone", ipaddr, "1", device, null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthAsyncResultModel, AuthAsyncResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth
        /// </summary>
        /// <param name="username">Unique identifier for the user that is commonly specified by your application during user creation (e.g. user@domain.com). Exactly one of user_id and username must be specified.</param>
        /// <param name="device">ID of the device to call. This device must have the "phone" capability. You may also specify "auto" to use the first of the user's devices with the "phone" capability.</param>
        /// <param name="ipaddr">The IP address of the user to be authenticated, in dotted quad format. This will cause an "allow" response to be sent if appropriate for requests from a trusted network.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthAsyncResult>> AuthPhoneByUsernameForPollingAsync(string username, string device = "auto", string ipaddr = null, CancellationToken cancelToken = default)
        {
            var request = builder.AuthRequest(null, username, "phone", ipaddr, "1", device, null);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthAsyncResultModel, AuthAsyncResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// https://duo.com/docs/authapi#/auth_status
        /// </summary>
        /// <param name="txid">The transaction ID of the authentication attempt, as returned by the /auth endpoint.</param>
        /// <param name="cancelToken">Cancellation Token in case you want to cancel mid-request.</param>
        public async Task<DuoResponse<AuthStatusResult>> AuthStatusAsync(string txid, CancellationToken cancelToken = default)
        {
            var request = builder.AuthStatusRequest(txid);
            var response = await client.SendAsync(request, cancelToken).ConfigureAwait(false);
            return await DuoResponse.ParseAsync<AuthStatusResultModel, AuthStatusResult>(response).ConfigureAwait(false);
        }

        public void Dispose()
        {
            if (this.ownsClient)
                this.client?.Dispose();
        }
    }
}
