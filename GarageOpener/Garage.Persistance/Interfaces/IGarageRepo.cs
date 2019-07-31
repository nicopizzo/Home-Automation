using Home.Core.Gpio;

namespace Garage.Persitance.Interfaces
{
    public interface IGarageRepo : IGpioCore
    {
        void ToggleGarage();
        GarageStatus GetGarageStatus();
    }
}
