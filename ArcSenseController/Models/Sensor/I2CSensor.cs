using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using ArcSenseController.Models.Sensor.Impl.Bme680;

namespace ArcSenseController.Models.Sensor
{
    internal abstract class I2CSensor : Sensor
    {
        protected internal I2cDevice Device { get; private set; }

        /// <summary>
        /// Called by the constructor or initialiser of the inheriting class
        /// to create and initialise the I2C interface.
        /// </summary>
        /// <param name="address">The slave address in hexadecimal.</param>
        protected async Task InitI2C(byte address)
        {
            var settings = new I2cConnectionSettings(address)
            {
                BusSpeed = I2cBusSpeed.FastMode,
                SharingMode = I2cSharingMode.Shared
            };

            var info = await DeviceInformation.FindAllAsync(I2cDevice.GetDeviceSelector());
            Device = await I2cDevice.FromIdAsync(info[0].Id, settings);
        }

        /// <summary>
        /// Reads data from the I2C device.
        /// </summary>
        /// <param name="reg">Read address.</param>
        /// <returns>Register data.</returns>
        internal uint ReadUint(byte reg)
        {
            var readBuffer = new byte[2];

            Device.WriteRead(new[] { reg }, readBuffer);
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
            var writeBuffer = new[] { reg };
            var readBuffer = new byte[1];

            Device.WriteRead(writeBuffer, readBuffer);

            return readBuffer[0];
        }
    }
}
