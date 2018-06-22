using DuoSecurity.Auth.Http.Abstraction;
using DuoSecurity.Auth.Http.JsonModels;
using DuoSecurity.Auth.Http.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http.Core
{
    public class DuoResponse<T>
    {
        public bool IsSuccessful { get; internal set; }

        public DuoError Error { get; internal set; }

        public HttpResponseMessage OriginalResponse { get; internal set; }

        public string OriginalJson { get; internal set; }

        public T Response { get; internal set; }

        internal DuoResponse() { }
    }

    internal static class DuoResponse
    {
        public static async Task<DuoResponse<Ty>> ParseAsync<Tx, Ty>(HttpResponseMessage response) where Tx : class, IModel<Ty>
        {
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var model = JsonConvert.DeserializeObject<BaseModel<Tx>>(content);
                return new DuoResponse<Ty>
                {
                    IsSuccessful = true,
                    Error = null,
                    OriginalResponse = response,
                    OriginalJson = content,
                    Response = model.Response.ToResult()
                };
            }

            return Error<Ty>(response, content);
        }

        public static async Task<DuoResponse<T>> ErrorAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Error<T>(response, content);
        }

        public static DuoResponse<T> Error<T>(HttpResponseMessage response, string content)
        {
            var error = JsonConvert.DeserializeObject<ErrorModel>(content);
            return new DuoResponse<T>
            {
                IsSuccessful = false,
                Error = new DuoError(error),
                OriginalResponse = response,
                OriginalJson = content,
                Response = default(T)
            };
        }
    }
}
