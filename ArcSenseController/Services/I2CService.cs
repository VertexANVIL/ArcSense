using System;
using ArcSenseController.Helpers;
using ArcSenseController.Models;

namespace ArcSenseController.Services
{
    public class I2CService
    {
        public IntPtr _bus;

        public void InitBus() {
            _bus = I2CNativeInterop.Open("/dev/i2c-1", I2CNativeInterop.OPEN_READ_WRITE);
            if (_bus.ToInt64() == 4294967295)
                throw new Exception("Failed to initialise bus!");
        }

        public void SetAddress(byte address) {
            var code = I2CNativeInterop.Ioctl(_bus, I2CNativeInterop.I2C_SLAVE, address);
        }

        public I2CDevice GetDeviceAt(byte address) {
            return new I2CDevice(this, address);
        }

        public void Read(byte register, byte[] buffer) {
            I2CNativeInterop.Read(_bus, buffer, buffer.Length);
        }

        public void Write(byte register, byte[] buffer) {
            I2CNativeInterop.Write(_bus, buffer, buffer.Length);
        }
    }
}