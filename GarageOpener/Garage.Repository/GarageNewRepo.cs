using Garage.Persitance;
using Garage.Persitance.Interfaces;
using Home.Core.Gpio;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace Garage.Repository
{
    public class GarageNewRepo : IGarageRepo
    {
        private readonly GarageConfig _config;
        private readonly ILogger<GarageRepo> _logger;

        public GarageNewRepo(GarageConfig config, ILogger<GarageRepo> logger)
        {
            _config = config;
            _logger = logger;
            Pi.Init<BootstrapWiringPi>();
            InitPins();
        }

        public GarageStatus GetGarageStatus()
        {
            var result = GarageStatus.Closed;
            _logger.Log(LogLevel.Information, "Getting Status...");

            var closedPin = Pi.Gpio[_config.ClosedPin];

            if (closedPin.Read()) result = GarageStatus.Open;

            _logger.Log(LogLevel.Information, $"Garage is {result}");
            return result;
        }

        public PinDetails GetPinDetails(int pin)
        {
            _logger.Log(LogLevel.Information, $"Getting Details on Pin {pin}");
            var p = Pi.Gpio[pin];
            return new PinDetails()
            {
                IsOpen = true,
                PinValue = p.Read() ? 1 : 0,
                PinNumber = pin,
                PinMode = p.PinMode == Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Input ? System.Device.Gpio.PinMode.Input : System.Device.Gpio.PinMode.Output
            };
        }

        public void SetPinDetails(PinDetails pinDetails)
        {
            try
            {
                var pin = Pi.Gpio[pinDetails.PinNumber];
                pin.PinMode = pinDetails.PinMode == System.Device.Gpio.PinMode.Input ? Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Input : Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
                if(pinDetails.PinMode == System.Device.Gpio.PinMode.Output)
                {
                    pin.Value = pinDetails.PinValue == 1;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to set pin details.");
                throw;
            }
        }

        public void ToggleGarage()
        {
            _logger.Log(LogLevel.Information, "Toggling Garage...");
            _logger.Log(LogLevel.Debug, $"Pin {_config.TogglePin} is Open... Setting Low");
            Pi.Gpio[_config.TogglePin].Value = false;
            Task.Delay(650).Wait();
            _logger.Log(LogLevel.Debug, $"Setting Pin {_config.TogglePin} High");
            Pi.Gpio[_config.TogglePin].Value = true;
        }

        private void InitPins()
        {
            _logger.LogInformation("Initializing pins");
            try
            {
                Pi.Gpio[_config.TogglePin].PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
                Pi.Gpio[_config.TogglePin].Value = true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unable to init pins");
            }
        }
    }
}
