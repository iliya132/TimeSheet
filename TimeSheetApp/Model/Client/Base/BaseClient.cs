using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.Client.Base
{
    public abstract class BaseClient
    {
        protected HttpClient Client;

        protected abstract string ServiceAddress { get; set; }

        protected BaseClient(IConfiguration configuration)
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(configuration["ClientAdress"])
            };
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
        }
        protected T Get<T>(string url) where T : new()
        {
            var result = new T();
            var response = Client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<T>().Result;
            }
            return result;
        }
        protected async Task<T> GetAsync<T>(string url) where T : new()
        {
            var list = new T();
            var response = await Client.GetAsync(url);
            if (response.IsSuccessStatusCode)
                list = await response.Content.ReadAsAsync<T>();
            return list;
        }
        protected HttpResponseMessage Post<T>(string url, T value)
        {
            var response = Client.PostAsJsonAsync(url, value).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T
        value)
        {
            var response = await Client.PostAsJsonAsync(url, value);
            response.EnsureSuccessStatusCode();
            return response;
        }
        protected HttpResponseMessage Put<T>(string url, T value)
        {
            var response = Client.PutAsJsonAsync(url, value).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T
        value)
        {
            var response = await Client.PutAsJsonAsync(url, value);
            response.EnsureSuccessStatusCode();
            return response;
        }
        protected HttpResponseMessage Delete(string url)
        {
            var response = Client.DeleteAsync(url).Result;
            return response;
        }
        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var response = await Client.DeleteAsync(url);
            return response;
        }

    }
}
