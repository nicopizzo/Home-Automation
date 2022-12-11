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

        public string BaseAddress { get; set; }
        private string _Token;
        public string Token
        {
            get => _Token;
            set
            {
                _Token = value;
                _Client.DefaultRequestHeaders.Clear();
                _Client.DefaultRequestHeaders.Add("Token", value);
            }
        }

        public ClientBase(string baseUrl, string token)
        {
            _Client = new HttpClient();
            BaseAddress = baseUrl;
            Token = token;
        }

        public ClientBase(HttpClient client)
        {
            _Client = client;
        }

        protected async Task<T> GetRequest<T>(string url)
        {
            var resp = await _Client.GetAsync($"{BaseAddress}{url}");
            return await ProcessRequest<T>(resp);
        }

        protected async Task<T> PostRequest<T>(string url, object obj)
        {
            var content = JsonConvert.SerializeObject(obj);
            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var resp = await _Client.PostAsync($"{BaseAddress}{url}", byteContent);
            return await ProcessRequest<T>(resp);
        }

        protected async Task<T> PutRequest<T>(string url)
        {
            var content = new StringContent("void");
            var resp = await _Client.PutAsync($"{BaseAddress}{url}", content);
            return await ProcessRequest<T>(resp);
        }

        protected async Task<T> PutRequest<T>(string url, HttpContent content)
        {
            var resp = await _Client.PutAsync($"{BaseAddress}{url}", content);
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
