using Garage.Persitance;
using Garage.Persitance.Interfaces;
using Garage.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Text;

namespace Garage.UnitTests
{
    [TestClass]
    public class BaseTests
    {
        protected GarageConfig _GarageConfig;
        protected Mock<IGpioController> _MockGpioController;
        protected Mock<AbstractLogger<GarageRepo>> _MockLogger;
        private Dictionary<int, bool> _PinMappings;
        private Dictionary<int, bool> _PinOpenMappings;
        private Dictionary<int, bool> _PinModeMappings;

        [TestInitialize]
        public void InitTests()
        {
            _PinMappings = new Dictionary<int, bool>();
            _PinOpenMappings = new Dictionary<int, bool>();
            _PinModeMappings = new Dictionary<int, bool>();

            _GarageConfig = new GarageConfig() { TogglePin = 4, OpenPin = 17, ClosedPin = 27 };

            _MockGpioController = new Mock<IGpioController>();
            _MockLogger = new Mock<AbstractLogger<GarageRepo>>();

            _MockLogger.Setup(f => f.Log(LogLevel.Information, It.IsAny<Exception>(), It.IsAny<string>()));

            _MockGpioController.Setup(f => f.IsPinOpen(It.IsAny<int>())).Returns((int x) => GetPinOpenValue(x));
            _MockGpioController.Setup(f => f.OpenPin(It.IsAny<int>())).Callback((int x) => ModifyPinOpen(x, true));
            _MockGpioController.Setup(f => f.ClosePin(It.IsAny<int>())).Callback((int x) => ModifyPinOpen(x, false));
            _MockGpioController.Setup(f => f.SetPinMode(It.IsAny<int>(), It.IsAny<PinMode>())).Callback((int pin, PinMode mode) => ModifyPinMode(pin, mode));
            _MockGpioController.Setup(f => f.Write(It.IsAny<int>(), It.IsAny<PinValue>())).Callback((int pin, PinValue value) => ModifyPin(pin, value));
        }

        protected PinValue GetPinValue(int pin)
        {
            return GetDictionaryValue(_PinMappings, pin) ? PinValue.High : PinValue.Low;
        }

        protected bool GetPinOpenValue(int pin)
        {
            return GetDictionaryValue(_PinOpenMappings, pin);
        }

        protected PinMode GetPinMode(int pin)
        {
            return GetDictionaryValue(_PinModeMappings, pin) ? PinMode.Output : PinMode.Input;
        }

        protected void ModifyPinOpen(int pin, bool value)
        {
            ModifyDictionary(_PinOpenMappings, pin, value);
        }

        protected void ModifyPin(int pin, PinValue value)
        {
            ModifyDictionary(_PinMappings, pin, value == PinValue.High);
        }

        protected void ModifyPinMode(int pin, PinMode mode)
        {
            ModifyDictionary(_PinModeMappings, pin, mode == PinMode.Output);
        }

        private void ModifyDictionary(Dictionary<int, bool> dict, int pin, bool value)
        {
            if (!dict.ContainsKey(pin))
            {
                dict.Add(pin, value);
            }
            else
            {
                dict[pin] = value;
            }
        }

        private bool GetDictionaryValue(Dictionary<int, bool> dict, int pin)
        {
            if (!dict.ContainsKey(pin))
            {
                return false;
            }
            return dict[pin];
        }
    }
}
