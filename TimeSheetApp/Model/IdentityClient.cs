using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TimeSheetApp.Model.Client.Base;
using TimeSheetApp.Model.Interfaces;

namespace TimeSheetApp.Model
{
    public class IdentityClient : BaseClient, IIdentityProvider
    {

        protected override string ServiceAddress { get; set; }

        public IdentityClient()
        {
#if DevAtHome
            ServiceAddress = @"http://localhost:8081/account";
#else
            ServiceAddress = @"http://172.25.100.210:81/account";
#endif
        }

        public Task LoginAsync(string login, string password)
        {
            string url = GenerateUrl(nameof(Login), $"login={login}&passwrd={password}");
            return Task.Run(() => Get(url));
        }

        public void Login(string login, string password)
        {
            string url = GenerateUrl(nameof(Login), $"login={login}& passwrd={password}");
            Get(url);
        }
    }
}
