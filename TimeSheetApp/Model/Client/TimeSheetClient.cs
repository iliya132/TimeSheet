using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetApp.Model.Client.Base;

namespace TimeSheetApp.Model.Client
{
    public class TimeSheetClient : BaseClient
    {
        protected override string ServiceAddress { get; set; }

        public TimeSheetClient(IConfiguration configuration) :base(configuration)
        {
            
        }
    }
}
