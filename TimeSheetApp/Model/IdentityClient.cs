using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using TimeSheetApp.Model.Client.Base;
using TimeSheetApp.Model.Interfaces;

namespace TimeSheetApp.Model
{
    public class IdentityClient : IIdentityProvider
    {

        protected string ServiceAddress { get; set; }
        private static string Token { get; set; }
        private HttpClient Client { get; set; }


        public IdentityClient()
        {
#if DevAtHome
            ServiceAddress = @"http://localhost:8081";
#else
            ServiceAddress = @"http://172.25.100.210:81";
#endif
            Client = new HttpClient();
        }

        public string GetToken()
        {
            if(string.IsNullOrWhiteSpace(Token) || !CanConnect())
            {
                try
                {
                    Login("TimeSheetUser", "DK_User1!");
                }
                catch
                {
                    Token = string.Empty;
                }
            }
            return Token;
        }

        private bool CanConnect()
        {
            return Client.GetAsync($"{ServiceAddress}/timesheet/conn").Result.IsSuccessStatusCode;
        }

        public class Credentials
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }

        private void Login(string login, string password)
        {
            string url = $"{ServiceAddress}/account/token";
            Credentials credentials = new Credentials
            {
                Name = login,
                Password = password
            };
            string json = JsonConvert.SerializeObject(credentials);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = Client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                string access_token = JObject.Parse(response.Content.ReadAsStringAsync().Result)["access_token"].ToString();
                Token = access_token;
            }
        }
    }
}
