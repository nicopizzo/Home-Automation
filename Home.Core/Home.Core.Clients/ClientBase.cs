using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Home.Core.Clients
{
    public abstract class ClientBase : IDisposable
    {
        protected readonly HttpClient _Client;

        public ClientBase(string baseUrl, string token)
        {
            _Client = new HttpClient();
            UpdateBaseAndToken(baseUrl, token);
        }

        public ClientBase(HttpClient client)
        {
            _Client = client;
        }

        public void UpdateBaseAndToken(string baseUrl, string token)
        {
            _Client.BaseAddress = new Uri(baseUrl);
            _Client.DefaultRequestHeaders.Clear();
            _Client.DefaultRequestHeaders.Add("Token", token);
        }

        protected async Task<T> GetRequest<T>(string url)
        {
            var resp = await _Client.GetAsync(url);
            return await ProcessRequest<T>(resp);
        }

        protected async Task<T> PostRequest<T>(string url, object obj)
        {
            var content = JsonConvert.SerializeObject(obj);
            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var resp = await _Client.PostAsync(url, byteContent);
            return await ProcessRequest<T>(resp);
        }

        protected async Task<T> PutRequest<T>(string url)
        {
            var content = new StringContent("void");
            var resp = await _Client.PutAsync(url, content);
            return await ProcessRequest<T>(resp);
        }

        protected async Task<T> PutRequest<T>(string url, HttpContent content)
        {
            var resp = await _Client.PutAsync(url, content);
            return await ProcessRequest<T>(resp);
        }

        private async Task<T> ProcessRequest<T>(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode) throw new HttpRequestException(httpResponse.ReasonPhrase);
            var content = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public void Dispose()
        {
            _Client?.Dispose();
        }
    }
}
