using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DuoSecurity.Auth.Http.Core
{
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

    internal static class DuoApiHelper
    {
        public static string CanonicalizeParams(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var ret = new List<string>();
            foreach (var pair in parameters)
            {
                string p = string.Format("{0}={1}",
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

        public static string CanonicalizeRequest(string method, string host, string endpoint, string canonParams, string date)
        {
            var lines = new string[]
            {
                date,
                method.ToUpperInvariant(),
                host.ToLower(),
                endpoint,
                canonParams
            };

            return string.Join("\n", lines);
        }

        public static string HmacSign(string secretKey, string data)
        {
            byte[] key_bytes = Encoding.ASCII.GetBytes(secretKey);
            HMACSHA1 hmac = new HMACSHA1(key_bytes);

            byte[] data_bytes = Encoding.ASCII.GetBytes(data);
            hmac.ComputeHash(data_bytes);

            string hex = BitConverter.ToString(hmac.Hash);
            return hex.Replace("-", "").ToLower();
        }

        public static string Encode64(string plaintext)
        {
            byte[] plaintext_bytes = Encoding.ASCII.GetBytes(plaintext);
            string encoded = Convert.ToBase64String(plaintext_bytes);
            return encoded;
        }

        public static string DateToRFC822(DateTime date)
        {
            // Can't use the "zzzz" format because it adds a ":"
            // between the offset's hours and minutes.
            string date_string = date.ToString(
                "ddd, dd MMM yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            var offset = TimeZoneInfo.Local.GetUtcOffset(date);
            string zone;

            // + or -, then 0-pad, then offset, then more 0-padding.
            if (offset.TotalHours < 0)
            {
                zone = "-";
            }
            else
            {
                zone = "+";
            }

            var offsetHours = offset.Hours;
            if (offsetHours < 0)
                offsetHours *= -1;

            zone += offsetHours.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');

            var offsetMinutes = offset.Minutes;
            if (offsetMinutes < 0)
                offsetMinutes *= -1;

            zone += offsetMinutes.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');

            date_string += " " + zone;
            return date_string;
        }
    }
}
