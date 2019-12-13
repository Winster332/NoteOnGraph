using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NoteOnGraph.Client.Api.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> GetResponseDataAsync<T>(this HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var projects = JsonConvert.DeserializeObject<T>(content);

            return projects;
        }
        
        public static T GetResponseData<T>(this HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var projects = JsonConvert.DeserializeObject<T>(content);

            return projects;
        }
        
        public static string GetContentAsString(this HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return content;
        }
        
        public static Guid GetContentAsGuid(this HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult()
                .Replace("\"", "");

            return Guid.Parse(content);
        }
    }
}