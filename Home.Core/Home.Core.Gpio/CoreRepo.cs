using System.Device.Gpio;

namespace Home.Core.Gpio
{
    public abstract class CoreRepo : IGpioCore
    {
        protected GpioController _gpioController;

        public CoreRepo(GpioController gpioController)
        {
            _gpioController = gpioController;
        }

        public PinDetails GetPinDetails(int pin)
        {
            var pinOpen = _gpioController.IsPinOpen(pin);
            if (!pinOpen) _gpioController.OpenPin(pin);
            var details = new PinDetails()
            {
                PinNumber = pin,
                IsOpen = pinOpen,
                PinValue = (int)_gpioController.Read(pin),
                PinMode = _gpioController.GetPinMode(pin)
            };
            if (!pinOpen) _gpioController.ClosePin(pin);
            return details;
        }

        public void SetPinDetails(PinDetails pinDetails)
        {
            var pinOpen = _gpioController.IsPinOpen(pinDetails.PinNumber);
            if (!pinOpen) _gpioController.OpenPin(pinDetails.PinNumber);
            _gpioController.Write(pinDetails.PinNumber, pinDetails.PinValue);
            _gpioController.SetPinMode(pinDetails.PinNumber, pinDetails.PinMode);
            if (!pinOpen) _gpioController.ClosePin(pinDetails.PinNumber);
        }
    }
}
