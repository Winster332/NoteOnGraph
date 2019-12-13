using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NoteOnGraph.IntegrationTests
{
    public class RestAPI
    {
        public HttpClient Client { get; set; }
        
        public RestAPI(HttpClient client)
        {
            Client = client;
        }

        public async Task<HttpResponseMessage> Put<T>(string url, T value)
        {
            var content = new StringContent(JsonConvert.SerializeObject(value));
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = content;
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
            var response = await Client.SendAsync(request);

            return response;
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            var response = await Client.GetAsync(url);

            return response;
        }

        public async Task<HttpResponseMessage> Delete(string url, Guid id)
        {
            var response = await Client.DeleteAsync($"{url}/{id}");

            return response;
        }

        public async Task<HttpResponseMessage> Post<T>(string url, T value = null) where T : class
        {
            var content = new StringContent(JsonConvert.SerializeObject(value));
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (value != null)
            {
                request.Content = content;
            }

            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
            var response = await Client.SendAsync(request);

            return response;
        }
    }
}