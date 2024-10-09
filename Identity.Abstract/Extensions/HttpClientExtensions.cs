using Identity.Abstract.Exceptions;
using Newtonsoft.Json;
using System.Text;

namespace Identity.Abstract.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T?> GetObjectAsync<T>(this HttpClient client, string route)
        {
            HttpResponseMessage response = await client.GetAsync(route);

            if (response.IsSuccessStatusCode)
            {
                string stringData = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(stringData);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                string stringData = await response.Content.ReadAsStringAsync();
                BadRequestResponse? badRequestResponse = JsonConvert.DeserializeObject<BadRequestResponse>(stringData);

                throw new InvalidRequestException(badRequestResponse?.Detail ?? "Invalid request.");
            }

            return default;
        }
        public static StringContent ConvertToStringContent<TRequest>(this TRequest request)
        {
            string jsonString = JsonConvert.SerializeObject(request);
            return new StringContent(jsonString, Encoding.UTF8, "application/json");
        }
    }
    public record BadRequestResponse
    {
        /// <summary>
        /// Detail
        /// </summary>
        public string? Detail { get; set; }
    }
}
