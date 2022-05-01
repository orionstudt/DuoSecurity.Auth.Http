using DuoSecurity.Auth.Http.Core;
using DuoSecurity.Auth.Http.JsonModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace DuoSecurity.Auth.Http
{
    internal static class DuoResponse
    {
        public static async Task<DuoResponse<TResult>> ParseAsync<TJsonModel, TResult>(HttpResponseMessage response)
            where TJsonModel : class, IJsonModel<TResult>
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var model = JsonConvert.DeserializeObject<BaseModel<TJsonModel>>(content);
                return new DuoResponse<TResult>
                {
                    IsSuccessful = true,
                    Error = null,
                    OriginalResponse = response,
                    OriginalJson = content,
                    Result = model.Response.ToResult()
                };
            }

            return Error<TResult>(response, content);
        }

        public static async Task<DuoResponse<TResult>> ErrorAsync<TResult>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return Error<TResult>(response, content);
        }

        public static DuoResponse<TResult> Error<TResult>(HttpResponseMessage response, string content)
        {
            var error = JsonConvert.DeserializeObject<ErrorModel>(content);
            return new DuoResponse<TResult>
            {
                IsSuccessful = false,
                Error = new DuoError(error),
                OriginalResponse = response,
                OriginalJson = content,
                Result = default(TResult),
            };
        }
    }
}
