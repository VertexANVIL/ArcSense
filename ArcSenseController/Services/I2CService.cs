using System;
using System.Linq;
using ArcSenseController.Helpers;
using ArcSenseController.Models;

namespace ArcSenseController.Services
{
    public class I2CService
    {
        public IntPtr _bus;
        public bool Initialised { get; private set; }

        public void InitBus() {
            _bus = I2CNativeInterop.Open("/dev/i2c-1", I2CNativeInterop.OPEN_READ_WRITE);
            if (_bus.ToInt64() == 4294967295)
                throw new Exception("Failed to initialise bus!");
            Initialised = true;
        }

        public I2CDevice GetDeviceAt(byte address) {
            if (!Initialised)
                throw new Exception($"Cannot open device at address {BitConverter.ToString(new byte[]{address})}. I2C bus not ready.");
            return new I2CDevice(this, address);
        }

        public void Read(byte address, byte register, byte[] buffer) {
            var regBuf = new [] { register };

            I2CNativeInterop.Ioctl(_bus, I2CNativeInterop.I2C_SLAVE, address);
            I2CNativeInterop.Write(_bus, regBuf, regBuf.Length);
            I2CNativeInterop.Read(_bus, buffer, buffer.Length);
        }

        public void Write(byte address, byte register, byte[] buffer) {
            var regBuf = new [] { register };
            var dataBuf = regBuf.Concat(buffer).ToArray();

            I2CNativeInterop.Ioctl(_bus, I2CNativeInterop.I2C_SLAVE, address);
            I2CNativeInterop.Write(_bus, dataBuf, buffer.Length);
        }

        public void WriteRead(byte address, byte register, byte[] write, byte[] buffer) {
            var regBuf = new [] { register };
            var dataBuf = regBuf.Concat(write).ToArray();

            I2CNativeInterop.Ioctl(_bus, I2CNativeInterop.I2C_SLAVE, address);
            I2CNativeInterop.Write(_bus, dataBuf, regBuf.Length);
            I2CNativeInterop.Read(_bus, buffer, buffer.Length);
        }
    }
}