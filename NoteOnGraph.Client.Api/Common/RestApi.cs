using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NoteOnGraph.Client.Api.Common
{
    public class RestApi
    {
        public HttpClient Client { get; set; }
        
        public RestApi(HttpClient client)
        {
            Client = client;
        }
        
        public async Task<RequestResult<TResult>> PutAsync<TResult, TValue>(string url, TValue value, Func<HttpResponseMessage, TResult> funcExtractValue)
        {
            var result = new RequestResult<TResult>();

            try
            {
                var response = default(HttpResponseMessage);
                var request = new HttpRequestMessage(HttpMethod.Put, url);

                if (value != null)
                {
                    var json = JsonConvert.SerializeObject(value).Replace("\"", "'");
                    var content = new StringContent(json);
                    request.Content = content;
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
                }

                response = await Client.SendAsync(request);

                result.Response = response;

                result.Result = funcExtractValue(response);
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }
        
        public async Task<RequestResult<TResult>> GetAsync<TResult>(string url, Func<HttpResponseMessage, TResult> funcExtractValue)
        {
            var result = new RequestResult<TResult>();

            try
            {
                var response = await Client.GetAsync(url);
                result.Response = response;
                result.Result = funcExtractValue(response);
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }
        
        public async Task<RequestResult<TResult>> DeleteAsync<TResult>(string url, Guid id, Func<HttpResponseMessage, TResult> funcExtractValue)
        {
            var result = new RequestResult<TResult>();

            try
            {
                var response = await Client.DeleteAsync($"{url}/{id}");
                result.Response = response;
                result.Result = funcExtractValue(response);
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

//        public async Task<HttpResponseMessage> PutAsync<T>(string url, T value)
//        {
//            var content = new StringContent(JsonConvert.SerializeObject(value));
//            var request = new HttpRequestMessage(HttpMethod.Put, url);
//            request.Content = content;
//            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
//            var response = await Client.SendAsync(request);
//
//            return response;
//        }

//        public async Task<HttpResponseMessage> GetAsync(string url)
//        {
//            var response = await Client.GetAsync(url);
//
//            return response;
//        }

//        public async Task<HttpResponseMessage> DeleteAsync(string url, Guid id)
//        {
//            var response = await Client.DeleteAsync($"{url}/{id}");

//            return response;
//        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T value = null) where T : class
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