using Microsoft.VisualStudio.TestTools.UnitTesting;
using Garage.Repository;
using Moq;
using Microsoft.Extensions.Logging;
using System;
using System.Device.Gpio;
using System.Threading.Tasks;

namespace Garage.UnitTests
{
    [TestClass]
    public class GarageRepoTests : BaseTests
    {
        [TestMethod]
        public void ToggleGarage_PinClosed_Test()
        {
            var repo = new GarageRepo(_GarageConfig, _MockLogger.Object);

            repo.ToggleGarage();

            Assert.IsTrue(GetPinValue(_GarageConfig.TogglePin) == PinValue.Low);
            Assert.IsFalse(GetPinOpenValue(_GarageConfig.TogglePin));
            _MockLogger.Verify(f => f.Log(LogLevel.Information, It.IsAny<Exception>(), "Toggling Garage..."), Times.Once);
            _MockGpioController.Verify(f => f.IsPinOpen(_GarageConfig.TogglePin), Times.Once);
            _MockGpioController.Verify(f => f.SetPinMode(_GarageConfig.TogglePin, PinMode.Output), Times.Once);
            _MockGpioController.Verify(f => f.Write(_GarageConfig.TogglePin, It.IsAny<PinValue>()), Times.Exactly(2));
            _MockGpioController.Verify(f => f.OpenPin(_GarageConfig.TogglePin), Times.Once);
            _MockGpioController.Verify(f => f.ClosePin(_GarageConfig.TogglePin), Times.Once);
        }

        [TestMethod]
        public async Task ToggleGarage_PinOpen_Test()
        {
            ModifyPinOpen(_GarageConfig.TogglePin, true);
            var repo = new GarageRepo(_GarageConfig, _MockLogger.Object);

            await repo.ToggleGarage();

            Assert.IsTrue(GetPinValue(_GarageConfig.TogglePin) == PinValue.Low);
            Assert.IsFalse(GetPinOpenValue(_GarageConfig.TogglePin));
            _MockLogger.Verify(f => f.Log(LogLevel.Information, It.IsAny<Exception>(), "Toggling Garage..."), Times.Once);
            _MockGpioController.Verify(f => f.IsPinOpen(_GarageConfig.TogglePin), Times.Once);
            _MockGpioController.Verify(f => f.SetPinMode(_GarageConfig.TogglePin, PinMode.Output), Times.Never);
            _MockGpioController.Verify(f => f.Write(_GarageConfig.TogglePin, It.IsAny<PinValue>()), Times.Exactly(2));
            _MockGpioController.Verify(f => f.OpenPin(_GarageConfig.TogglePin), Times.Never);
            _MockGpioController.Verify(f => f.ClosePin(_GarageConfig.TogglePin), Times.Once);
        }

        [TestMethod]
        public void GetGarageStatus_Open_Test()
        {
            //ModifyPin()
        }
    }
}
