using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeAutomationApp.Core
{
    public class GarageAppCore : AppCore
    {
        public GarageAppCore(string baseUrl, string token) : base(baseUrl, token)
        {

        }

        public async Task ToggleGarage()
        {
            var url = "/api/Garage/toggleGarage";
            await base.PutRequest<string>(url);
        }

        public async Task<int> GetGarageStatus()
        {
            var url = "/api/Garage/getGarageStatus";
            return await base.GetRequest<int>(url);
        }
    }
}
