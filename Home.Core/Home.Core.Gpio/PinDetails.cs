using System;
using System.Collections.Generic;
using System.Text;
using System.Device.Gpio;

namespace Home.Core.Gpio
{
    public class PinDetails
    {
        public int PinNumber { get; set; }
        public int PinValue { get; set; }
        public PinMode PinMode { get; set; }
        public bool IsOpen { get; set; }
    }
}
