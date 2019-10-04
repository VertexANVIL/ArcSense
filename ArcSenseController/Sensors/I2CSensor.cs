using System;
using System.Threading.Tasks;
using ArcSenseController.Helpers;
using ArcSenseController.Services;
using ArcSenseController.Models;

namespace ArcSenseController.Sensors
{
    internal abstract class I2CSensor : Sensor
    {
        private readonly I2CService _i2c;
        public I2CDevice Device;
        internal I2CSensor(I2CService i2c) {
            _i2c = i2c;
        }

        protected void InitI2C(byte address) {
            Device = _i2c.GetDeviceAt(address);
        }

        /// <summary>
        /// Reads data from the I2C device.
        /// </summary>
        /// <param name="reg">Read address.</param>
        /// <returns>Register data.</returns>
        internal uint ReadUint(byte reg)
        {
            var readBuffer = new byte[2];

            Device.Read(reg, readBuffer);
            var value = (uint)((readBuffer[1] << 8) + readBuffer[0]);

            return value;
        }

        /// <summary>
        /// Reads data from the I2C device.
        /// </summary>
        /// <param name="reg">Read address</param>
        /// <returns>Register data.</returns>
        protected byte ReadByte(byte reg)
        {
            var readBuffer = new byte[1];
            Device.Read(reg, readBuffer);

            return readBuffer[0];
        }
    }
}
