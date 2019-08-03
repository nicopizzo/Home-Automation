using Microsoft.Extensions.Logging;
using System;
using System.Device.Gpio;

namespace Home.Core.Gpio
{
    public class BashGpioController : IGpioController
    {
        public int PinCount { get; set; } = 40;

        private readonly string _Command;
        private readonly ILogger<BashGpioController> _Logger;

        public BashGpioController(ILogger<BashGpioController> logger)
        {
            _Command = "gpio {0} {1} {2}";
            _Logger = logger;
        }

        public BashGpioController(ILogger<BashGpioController> logger, string command)
        {
            _Command = command;
            _Logger = logger;
        }

        public void ClosePin(int pinNumber)
        {
        }

        public void Dispose()
        {
        }

        public PinMode GetPinMode(int pinNumber)
        {
            return PinMode.Input;
        }

        public bool IsPinModeSupported(int pinNumber, PinMode mode)
        {
            throw new NotImplementedException();
        }

        public bool IsPinOpen(int pinNumber)
        {
            return true;
        }

        public void OpenPin(int pinNumber)
        {
        }

        public void OpenPin(int pinNumber, PinMode mode)
        {
            SetPinMode(pinNumber, mode);
        }

        public PinValue Read(int pinNumber)
        {
            var command = string.Format(_Command, GpioCommands.Read, pinNumber.ToString(), "");
            _Logger.Log(LogLevel.Debug, $"Reading Pin {pinNumber} with command: {command}");
            var value = command.Bash()?.Substring(0, 1);
            _Logger.Log(LogLevel.Debug, $"Raw value is {value}");
            return value == "1" ? PinValue.High : PinValue.Low;
        }

        public void Read(Span<PinValuePair> pinValuePairs)
        {
            foreach(var p in pinValuePairs)
            {
                Read(p.PinNumber);
            }
        }

        public void SetPinMode(int pinNumber, PinMode mode)
        {
            var command = string.Format(_Command, GpioCommands.Mode, pinNumber, mode == PinMode.Input ? GpioMode.In : GpioMode.Out);
            _Logger.Log(LogLevel.Debug, $"Setting Pin mode on pin {pinNumber} with command: {command}");
            command.Bash();
        }

        public void Write(int pinNumber, PinValue value)
        {
            var command = string.Format(_Command, GpioCommands.Write, pinNumber.ToString(), value == PinValue.High ? GpioValue.High : GpioValue.Low);
            _Logger.Log(LogLevel.Debug, $"Setting Pin Value on pin {pinNumber} with command: {command}");
            command.Bash();
        }

        public void Write(ReadOnlySpan<PinValuePair> pinValuePairs)
        {
            foreach(var p in pinValuePairs)
            {
                Write(p.PinNumber, p.PinValue);
            }
        }
    }
}
