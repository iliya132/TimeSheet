﻿using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeSheetApp.Model.Client.Base
{
    public abstract class BaseClient
    {
        protected HttpClient Client;

        protected abstract string ServiceAddress { get; set; }

        protected BaseClient()
        {
            Client = new HttpClient();
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
                result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            return result;
        }

        protected void Get(string url)
        {
            _ = Client.GetAsync(url).Result;
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
