using Garage.Persitance;
using Garage.Persitance.Interfaces;
using Home.Core.Gpio;
using Microsoft.Extensions.Logging;
using System.Device.Gpio;
using System.Threading.Tasks;

namespace Garage.Repository
{
    public class GarageRepo : CoreRepo, IGarageRepo
    {
        private readonly GarageConfig _config;
        private readonly ILogger<GarageRepo> _logger;

        public GarageRepo(IGpioController gpioController, GarageConfig config, ILogger<GarageRepo> logger) : base(gpioController)
        {
            _config = config;
            _logger = logger;
        }

        public void ToggleGarage()
        {
            _logger.Log(LogLevel.Information, "Toggling Garage...");
            if (!_gpioController.IsPinOpen(_config.TogglePin))
            {
                _logger.Log(LogLevel.Debug, $"Pin {_config.TogglePin} is not open... Opening");
                _gpioController.OpenPin(_config.TogglePin, PinMode.Output);
            }

            _logger.Log(LogLevel.Debug, $"Pin {_config.TogglePin} is Open... Setting Low");
            _gpioController.Write(_config.TogglePin, PinValue.Low);
            Task.Delay(650).Wait();
            _logger.Log(LogLevel.Debug, $"Setting Pin {_config.TogglePin} High");
            _gpioController.Write(_config.TogglePin, PinValue.High);

            _logger.Log(LogLevel.Debug, $"Closing Pin {_config.TogglePin}");
            _gpioController.ClosePin(_config.TogglePin);
        }

        public GarageStatus GetGarageStatus()
        {
            var result = GarageStatus.Closed;
            _logger.Log(LogLevel.Information, "Getting Status...");

            if (!_gpioController.IsPinOpen(_config.ClosedPin))
            {
                _gpioController.OpenPin(_config.ClosedPin);
                _gpioController.SetPinMode(_config.ClosedPin, PinMode.Input);
            }

            var rawResult = _gpioController.Read(_config.ClosedPin);
            _logger.Log(LogLevel.Information, $"Pin in {rawResult}");
            if (rawResult == PinValue.High) result = GarageStatus.Open;

            _gpioController.ClosePin(_config.ClosedPin);

            _logger.Log(LogLevel.Information, $"Garage is {result}");
            return result;
        }
    }
}
