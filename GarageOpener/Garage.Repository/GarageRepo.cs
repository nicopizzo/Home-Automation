using Garage.Persitance;
using Garage.Persitance.Interfaces;
using Home.Core.Gpio;
using Microsoft.Extensions.Logging;
using System;
using System.Device.Gpio;
using System.Threading.Tasks;

namespace Garage.Repository
{
    public class GarageRepo : IGarageRepo, IDisposable
    {
        private readonly GarageConfig _config;
        private readonly ILogger<GarageRepo> _logger;
        private readonly GpioController _GpioController;

        public GarageRepo(GarageConfig config, ILogger<GarageRepo> logger)
        {
            _config = config;
            _logger = logger;
            _GpioController = new GpioController();
            InitPins();
        }

        public GarageStatus GetGarageStatus()
        {
            var result = GarageStatus.Closed;
            _logger.Log(LogLevel.Information, "Getting Status...");

            var closedPin = _GpioController.Read(_config.ClosedPin);

            if (closedPin == PinValue.High) result = GarageStatus.Open;

            _logger.Log(LogLevel.Information, $"Garage is {result}");
            return result;
        }

        public PinDetails GetPinDetails(int pin)
        {
            _logger.Log(LogLevel.Information, $"Getting Details on Pin {pin}");
            var p = _GpioController.Read(pin);
            var mode = _GpioController.GetPinMode(pin);
            return new PinDetails()
            {
                IsOpen = true,
                PinValue = p == PinValue.High ? 1 : 0,
                PinNumber = pin,
                PinMode = mode
            };
        }

        public void SetPinDetails(PinDetails pinDetails)
        {
            try
            {
                if (!_GpioController.IsPinOpen(pinDetails.PinNumber))
                {
                    _GpioController.OpenPin(pinDetails.PinNumber);
                }
                var pin = _GpioController.GetPinMode(pinDetails.PinNumber);
                _GpioController.SetPinMode(pinDetails.PinNumber, pinDetails.PinMode);
                if(pinDetails.PinMode == PinMode.Output)
                {
                    _GpioController.Write(pinDetails.PinNumber, pinDetails.PinValue);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to set pin details.");
                throw;
            }
        }

        public async Task ToggleGarage()
        {
            _logger.Log(LogLevel.Information, "Toggling Garage...");
            _logger.Log(LogLevel.Debug, $"Pin {_config.TogglePin} is Open... Setting Low");
            _GpioController.Write(_config.TogglePin, PinValue.Low);
            await Task.Delay(650);
            _logger.Log(LogLevel.Debug, $"Setting Pin {_config.TogglePin} High");
            _GpioController.Write(_config.TogglePin, PinValue.High);
        }

        private void InitPins()
        {
            _logger.LogInformation("Initializing pins");
            try
            {
                _GpioController.OpenPin(_config.TogglePin, PinMode.Output);
                _GpioController.Write(_config.TogglePin, PinValue.High);

                _GpioController.OpenPin(_config.ClosedPin, PinMode.Input);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unable to init pins");
            }
        }
        public void Dispose()
        {
            _GpioController?.Dispose();
        }
    }
}
