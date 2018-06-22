using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.Results
{
    public class LogoResult
    {
        /// <summary>
        /// MIME Type of image.
        /// </summary>
        public string ContentType { get; }

        /// <summary>
        /// Binary image data.
        /// </summary>
        public byte[] Data { get; }

        internal LogoResult(byte[] content)
        {
            ContentType = "image/png";
            Data = content;
        }
    }
}
