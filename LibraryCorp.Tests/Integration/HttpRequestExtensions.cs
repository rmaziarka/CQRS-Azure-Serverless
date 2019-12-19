using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LibraryCorp.Tests.Integration
{
    public static class HttpRequestExtensions
    {
        public static async Task<HttpResponseMessage> PostJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value)
        {
            var data = JsonConvert.SerializeObject(value);
            var content = new StringContent(data,
                Encoding.UTF8,
                "application/json");

            return await client.PostAsync(requestUri,
                    content)
                .ConfigureAwait(false);
        }
    }
}
