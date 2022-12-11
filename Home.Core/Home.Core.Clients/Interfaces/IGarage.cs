using System.Threading.Tasks;

namespace Home.Core.Clients.Interfaces
{
    public interface IGarage
    {
        string BaseAddress { get; set; }
        string Token { get; set; }

        Task ToggleGarage();
        Task<int> GetGarageStatus();
    }
}
