using System.Threading.Tasks;

namespace Home.Core.Clients
{
    public class GarageClient : ClientBase
    {
        public GarageClient(string baseUrl, string token) : base(baseUrl, token)
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
