using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DuoSecurity.Auth.Http.Core
{
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

        private HttpRequestMessage buildMessage(HttpMethod method, string endpoint, params KeyValuePair<string, string>[] parameters)
        {
            // Url Encoded Parameters
            var urlParams = string.Empty;
            if (parameters.Any()) urlParams = canonicalizeParams(parameters);

            // Date
            var dateStr = dateToRFC822(DateTime.Now);

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
            var signature = canonicalizeRequest(mUpper, endpoint, urlParams, dateStr);

            // Authorization
            var signed = hmacSign(signature);
            var auth = $"{integrationKey}:{signed}";
            message.Headers.Add("Authorization", $"Basic {encode64(auth)}");

            // Add Content Body if POST
            if (mUpper == "POST") message.Content = new FormUrlEncodedContent(parameters);

            return message;
        }

        public HttpRequestMessage PingRequest()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, $"https://{host}/{prefix}/ping");
            message.Headers.Add("Date", dateToRFC822(DateTime.Now));
            return message;
        }

        public HttpRequestMessage CheckRequest()
        {
            return buildMessage(HttpMethod.Get, "check");
        }

        public HttpRequestMessage LogoRequest()
        {
            return buildMessage(HttpMethod.Get, "logo");
        }

        public HttpRequestMessage EnrollRequest(string username, int? validSecs)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrWhiteSpace(username)) parameters.Add(new KeyValuePair<string, string>("username", username));
            if (validSecs.HasValue) parameters.Add(new KeyValuePair<string, string>("valid_secs", validSecs.Value.ToString()));
            return buildMessage(HttpMethod.Post, "enroll", parameters.ToArray());
        }

        public HttpRequestMessage EnrollCheckRequest(string userId, string activationCode)
        {
            return buildMessage(HttpMethod.Post, "enroll_status", new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("user_id", userId),
                new KeyValuePair<string, string>("activation_code", activationCode)
            });
        }

        public HttpRequestMessage PreAuthRequest(string userId, string username, string ipaddr, string trustedDeviceToken)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            // User Parameter
            if (!string.IsNullOrWhiteSpace(userId)) parameters.Add(new KeyValuePair<string, string>("user_id", userId));
            else if (!string.IsNullOrWhiteSpace(username)) parameters.Add(new KeyValuePair<string, string>("username", username));
            // Optional Parameters
            if (!string.IsNullOrWhiteSpace(ipaddr)) parameters.Add(new KeyValuePair<string, string>("ipaddr", ipaddr));           
            if (!string.IsNullOrWhiteSpace(trustedDeviceToken)) parameters.Add(new KeyValuePair<string, string>("trusted_device_token", trustedDeviceToken));
            return buildMessage(HttpMethod.Post, "preauth", parameters.ToArray());
        }

        public HttpRequestMessage AuthRequest(string userId, string username, string factor, string ipaddr, string async, string device, string passcode)
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("factor", factor)
            };
            // User Parameter
            if (!string.IsNullOrWhiteSpace(userId)) parameters.Add(new KeyValuePair<string, string>("user_id", userId));
            else if (!string.IsNullOrWhiteSpace(username)) parameters.Add(new KeyValuePair<string, string>("username", username));
            // Factor Parameter
            if (!string.IsNullOrWhiteSpace(device)) parameters.Add(new KeyValuePair<string, string>("device", device));
            else if (!string.IsNullOrWhiteSpace(passcode)) parameters.Add(new KeyValuePair<string, string>("passcode", passcode));
            // Optional Parameters
            if (!string.IsNullOrWhiteSpace(ipaddr)) parameters.Add(new KeyValuePair<string, string>("ipaddr", ipaddr));
            if (!string.IsNullOrWhiteSpace(async)) parameters.Add(new KeyValuePair<string, string>("async", async));            
            return buildMessage(HttpMethod.Post, "auth", parameters.ToArray());
        }

        public HttpRequestMessage AuthStatusRequest(string transactionId)
        {
            return buildMessage(HttpMethod.Get, "auth_status", new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("txid", transactionId)
            });
        }

        /* The following methods for building the HMAC Signature were taken from:
         * 
         * https://github.com/duosecurity/duo_api_csharp
         * 
         * License:
         *  Copyright (c) 2013, Duo Security, Inc.
            All rights reserved.

            Redistribution and use in source and binary forms, with or without
            modification, are permitted provided that the following conditions
            are met:

            1. Redistributions of source code must retain the above copyright
               notice, this list of conditions and the following disclaimer.
            2. Redistributions in binary form must reproduce the above copyright
               notice, this list of conditions and the following disclaimer in the
               documentation and/or other materials provided with the distribution.
            3. The name of the author may not be used to endorse or promote products
               derived from this software without specific prior written permission.

            THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
            IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
            OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
            IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
            INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
            NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
            DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
            THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
            (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
            THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
            */
        
        private string canonicalizeParams(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var ret = new List<string>();
            foreach (var pair in parameters)
            {
                string p = String.Format("{0}={1}",
                                         HttpUtility.UrlEncode(pair.Key),
                                         HttpUtility.UrlEncode(pair.Value));
                // Signatures require upper-case hex digits.
                p = Regex.Replace(p,
                                  "(%[0-9A-Fa-f][0-9A-Fa-f])",
                                  c => c.Value.ToUpperInvariant());
                // Escape only the expected characters.
                p = Regex.Replace(p,
                                  "([!'()*])",
                                  c => "%" + Convert.ToByte(c.Value[0]).ToString("X"));
                p = p.Replace("%7E", "~");
                // UrlEncode converts space (" ") to "+". The
                // signature algorithm requires "%20" instead. Actual
                // + has already been replaced with %2B.
                p = p.Replace("+", "%20");
                ret.Add(p);
            }
            ret.Sort(StringComparer.Ordinal);
            return string.Join("&", ret.ToArray());
        }
        
        private string canonicalizeRequest(string method, string endpoint, string canonParams, string date)
        {
            var lines = new string[]
            {
                date,
                method.ToUpperInvariant(),
                host.ToLower(),
                $"/{prefix}/{endpoint}",
                canonParams
            };
            return string.Join("\n", lines);
        }

        private string hmacSign(string data)
        {
            byte[] key_bytes = Encoding.ASCII.GetBytes(secretKey);
            HMACSHA1 hmac = new HMACSHA1(key_bytes);

            byte[] data_bytes = Encoding.ASCII.GetBytes(data);
            hmac.ComputeHash(data_bytes);

            string hex = BitConverter.ToString(hmac.Hash);
            return hex.Replace("-", "").ToLower();
        }

        private string encode64(string plaintext)
        {
            byte[] plaintext_bytes = Encoding.ASCII.GetBytes(plaintext);
            string encoded = Convert.ToBase64String(plaintext_bytes);
            return encoded;
        }

        private string dateToRFC822(DateTime date)
        {
            // Can't use the "zzzz" format because it adds a ":"
            // between the offset's hours and minutes.
            string date_string = date.ToString(
                "ddd, dd MMM yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            int offset = TimeZoneInfo.Local.GetUtcOffset(date).Hours;
            string zone;
            // + or -, then 0-pad, then offset, then more 0-padding.
            if (offset < 0)
            {
                offset *= -1;
                zone = "-";
            }
            else
            {
                zone = "+";
            }
            zone += offset.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            date_string += " " + zone.PadRight(5, '0');
            return date_string;
        }
    }
}
