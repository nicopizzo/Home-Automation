using System.Threading.Tasks;

namespace Home.Core.Clients.Interfaces
{
    public interface IGarage : IClientBase
    {
        Task ToggleGarage();
        Task<int> GetGarageStatus();
    }
}
