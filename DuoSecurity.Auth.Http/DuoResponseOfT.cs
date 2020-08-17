using System.Net.Http;
using DuoSecurity.Auth.Http.Core;

namespace DuoSecurity.Auth.Http
{
    public class DuoResponse<T>
    {
        public bool IsSuccessful { get; internal set; }

        public DuoError Error { get; internal set; }

        public HttpResponseMessage OriginalResponse { get; internal set; }

        public string OriginalJson { get; internal set; }

        public T Result { get; internal set; }

        internal DuoResponse() { }
    }
}
