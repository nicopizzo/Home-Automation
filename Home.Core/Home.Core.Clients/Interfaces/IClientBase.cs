using System;
using System.Collections.Generic;
using System.Text;

namespace Home.Core.Clients.Interfaces
{
    public interface IClientBase
    {
        void UpdateBaseAndToken(string baseUrl, string token);
    }
}
