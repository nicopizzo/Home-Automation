namespace Home.Core.Gpio
{
    public interface IGpioCore
    {
        PinDetails GetPinDetails(int pin);
        void SetPinDetails(PinDetails pinDetails);
    }
}
