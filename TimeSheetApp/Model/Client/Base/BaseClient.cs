using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeSheetApp.Model.Interfaces;

namespace TimeSheetApp.Model.Client.Base
{
    public abstract class BaseClient
    {
        protected HttpClient Client;
        private readonly IIdentityProvider identityClient;

        protected abstract string ServiceAddress { get; set; }

        protected BaseClient(IIdentityProvider identityClient)
        {
            this.identityClient = identityClient;
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected T Get<T>(string url) where T : new()
        {
            T result = new T();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", identityClient.GetToken());
            HttpResponseMessage response = Client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<T>().Result;
            }
            return result;
        }

        protected void Get(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", identityClient.GetToken());
            _ = Client.SendAsync(request).Result;
        }

        protected async Task<T> GetAsync<T>(string url) where T : new()
        {
            T result = new T();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", identityClient.GetToken());
            HttpResponseMessage response = await Client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<T>();
            return result;
        }

        protected HttpResponseMessage Post<T>(string url, T value)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", identityClient.GetToken());
            HttpContent content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            request.Content = content;
            HttpResponseMessage response = Client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T value)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", identityClient.GetToken());
            HttpContent content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            request.Content = content;
            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }

        protected HttpResponseMessage Put<T>(string url, T value)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", identityClient.GetToken());
            HttpContent content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            request.Content = content;
            HttpResponseMessage response = Client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T value)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", identityClient.GetToken());
            HttpContent content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            request.Content = content;
            HttpResponseMessage response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }

        protected HttpResponseMessage Delete(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", identityClient.GetToken());
            HttpResponseMessage response = Client.SendAsync(request).Result;
            return response;
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", identityClient.GetToken());
            HttpResponseMessage response = await Client.SendAsync(request);
            return response;
        }

        protected string GenerateUrl(string senderName, params string[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            if (parameters == null || parameters.Length == 0)
            {
                return $"{ServiceAddress}/{senderName}";
            }
            else
            {

                foreach (string param in parameters)
                {
                    sb.Append($"{param}&");
                }
                return $"{ServiceAddress}/{senderName}?{sb.Remove(sb.Length - 1, 1)}";
            }
        }

    }
}
