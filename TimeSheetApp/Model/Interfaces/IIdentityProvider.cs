using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.Interfaces
{
    public interface IIdentityProvider
    {
        string GetToken();
    }
}
