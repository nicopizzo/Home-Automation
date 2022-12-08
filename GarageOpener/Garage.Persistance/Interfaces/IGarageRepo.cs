using Home.Core.Gpio;
using System.Threading.Tasks;

namespace Garage.Persitance.Interfaces
{
    public interface IGarageRepo : IGpioCore
    {
        Task ToggleGarage();
        GarageStatus GetGarageStatus();
    }
}
