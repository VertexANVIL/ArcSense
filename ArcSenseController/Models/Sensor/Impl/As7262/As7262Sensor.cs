using System;
using System.Threading.Tasks;
using ArcDataCore.Models.Data;
using ArcDataCore.Models.Sensor;
using ArcSenseController.Models.Sensor.Types;
using MessagePack;

namespace ArcSenseController.Models.Sensor.Impl.As7262
{
    /// <summary>
    /// Implementation of the AS7262 visible spectrum device.
    /// </summary>
    internal sealed class As7262Sensor : I2CSensor, ISpectralSensor, ITemperatureSensor
    {
        private bool _dataReady = false;
        private As7262ConversionType _bank = As7262ConversionType.Default;
        private As7262ChannelGain _gain = As7262ChannelGain.Default;
        private bool _interrupt;
        private bool _reset;

        // Indicator and driver LEDs
        private bool _indicatorEnabled;
        private bool _driverEnabled;

        private As7262IndicatorCurrentLimit _indicatorCurrent = As7262IndicatorCurrentLimit.Limit1Ma;
        private As7262DriverCurrentLimit _driverCurrent = As7262DriverCurrentLimit.Limit12Ma5;

        #region Constants and Enums

        // I2C Slave Address
        private const byte AS7262_SLAVE_ADDRESS = 0x49;

        private const double INTEGRATION_TIME_COEFFICIENT = 2.8;
        private const double NUM_CHANNELS = 6;

        private enum As7262VirtualRegister : sbyte
        {
            HwVersion = 0x00,
            FwVersion = 0x02,
            ControlSetup = 0x04,
            IntT = 0x05,
            DeviceTemp = 0x06,
            LedControl = 0x07,

            // Raw data
            VHigh = 0x08,
            VLow = 0x09,
            BHigh = 0x0A,
            BLow = 0x0B,
            GHigh = 0x0C,
            GLow = 0x0D,
            YHigh = 0x0E,
            YLow = 0x0F,
            OHigh = 0x10,
            OLow = 0x11,
            RHigh = 0x12,
            RLow = 0x13,

            // Calibrated data
            VCal = 0x14,
            BCal = 0x18,
            GCal = 0x1C,
            YCal = 0x20,
            OCal = 0x24,
            RCal = 0x28
        }

        private enum As7262HardRegister : sbyte
        {
            StatusReg = 0x00,
            WriteReg = 0x01,
            ReadReg = 0x02,
            TxValid = 0x02,
            RxValid = 0x01,
        }

        #endregion

        public double Violet => ReadCalibratedChannel(As7262ChannelType.VioletCalibrated);
        public double Blue => ReadCalibratedChannel(As7262ChannelType.BlueCalibrated);
        public double Green => ReadCalibratedChannel(As7262ChannelType.GreenCalibrated);
        public double Yellow => ReadCalibratedChannel(As7262ChannelType.YellowCalibrated);
        public double Orange => ReadCalibratedChannel(As7262ChannelType.OrangeCalibrated);
        public double Red => ReadCalibratedChannel(As7262ChannelType.RedCalibrated);

        public override async Task InitialiseAsync()
        {
            await InitI2C(AS7262_SLAVE_ADDRESS);

            // Reset the sensor
            _reset = true;
            WriteControlRegister();
            _reset = false;

            // Wait for initialisation
            Task.Delay(1000).Wait();

            // Check version
            var version = VirtualRead((byte)As7262VirtualRegister.HwVersion);
            if (version != 0x40) throw new Exception("Unsupported AS726X hardware version.");

            _interrupt = true;
            WriteControlRegister();

            // Set up LEDs
            WriteLedRegister();

            // Set conversion parameters
            SetIntegrationTime(50);

            _gain = As7262ChannelGain.Gain64X;
            _bank = As7262ConversionType.Continuous2;
            WriteControlRegister();
        }

        public override SensorModel Model => SensorModel.As7262;

        private void EnableInterrupt()
        {
            _interrupt = true;
            WriteControlRegister();
        }

        private void ReadControlRegister()
        {
            throw new NotImplementedException();
            //var data = VirtualRead((byte) As7262VirtualRegister.ControlSetup);
            //_dataReady = data & 
        }

        private void WriteControlRegister()
        {
            // TIL: there needs to be enclosing brackets around ternary operations when in combination with bit shift :D
            var data = ((_dataReady ? 1 : 0) << 1) | ((byte)_bank << 2) | ((byte)_gain << 4) | ((_interrupt ? 1 : 0) << 6) | ((_reset ? 1 : 0) << 7);
            VirtualWrite((byte)As7262VirtualRegister.ControlSetup, (byte)data);
        }

        private void WriteLedRegister()
        {
            var data = (_indicatorEnabled ? 1 : 0) | ((sbyte)_indicatorCurrent << 1) | ((_driverEnabled ? 1 : 0) << 3) | ((sbyte)_driverCurrent << 4);
            VirtualWrite((byte)As7262VirtualRegister.LedControl, (byte)data);
        }

        /// <summary>
        /// Turns on or off / sets the current of the indicator LED.
        /// </summary>
        public void SetIndicator(bool state, As7262IndicatorCurrentLimit? current = null)
        {
            _indicatorEnabled = state;
            if (current.HasValue) _indicatorCurrent = current.Value;

            WriteLedRegister();
        }

        /// <summary>
        /// Turns on or off / sets the current of the driver LED.
        /// </summary>
        public void SetDriver(bool state, As7262DriverCurrentLimit? current = null)
        {
            _driverEnabled = state;
            if (current.HasValue) _driverCurrent = current.Value;

            WriteLedRegister();
        }

        /// <summary>
        /// Sets the current conversion type.
        /// </summary>
        public void SetConversionType(As7262ConversionType type)
        {
            _bank = type;
            WriteControlRegister();
        }

        public void SetGain(As7262ChannelGain gain)
        {
            _gain = gain;
            WriteControlRegister();
        }

        /// <summary>
        /// Sets the integration time for the sensor.
        /// </summary>
        /// <param name="time">The time to set in milliseconds. The time will be multiplied by 2.8ms.</param>
        public void SetIntegrationTime(ushort time)
        {
            VirtualWrite((byte)As7262VirtualRegister.IntT, (byte)time);
        }

        /// <summary>
        /// Reads a raw spectral channel.
        /// </summary>
        /// <param name="channel">The channel to read.</param>
        /// <returns>The reading as an unsigned integer.</returns>
        private ushort ReadChannel(As7262ChannelType channel)
        {
            return (ushort)((VirtualRead((byte) channel) << 8) | (byte)(channel + 1));
        }

        /// <summary>
        /// Reads a calibrated spectral channel.
        /// </summary>
        /// <param name="channel">The channel to read.</param>
        /// <returns>The reading as a float.</returns>
        private float ReadCalibratedChannel(As7262ChannelType channel)
        {
            var result = (VirtualRead((byte) channel) << 24) | 
                         (VirtualRead((byte) (channel + 1)) << 16) | 
                         (VirtualRead((byte) (channel + 2)) << 8) | 
                         (VirtualRead((byte) (channel + 3)));

            return result;
        }

        /// <summary>
        /// Reads the temperature of the integrated temperature sensor.
        /// </summary>
        private double ReadTemperature()
        {
            return VirtualRead((byte) As7262VirtualRegister.DeviceTemp);
        }

        public double Temperature => ReadTemperature();

        private byte VirtualRead(byte address)
        {
            while (true)
            {
                // spinlock until we can tx address
                var status = ReadByte((byte)As7262HardRegister.StatusReg);
                if ((status & (byte)As7262HardRegister.TxValid) == 0) break;
            }

            Device.Write(new[] { (byte)As7262HardRegister.WriteReg, address, (byte)1 });

            while (true)
            {
                // spinlock until we can rx data
                var status = ReadByte((byte)As7262HardRegister.StatusReg);
                if ((status & (byte)As7262HardRegister.RxValid) != 0) break;
            }

            var output = new byte[1];
            Device.WriteRead(new[] { (byte)As7262HardRegister.ReadReg, (byte)1 }, output);

            return output[0];
        }

        private void VirtualWrite(byte address, byte value)
        {
            while (true)
            {
                // spinlock until we can tx address
                var status = ReadByte((byte)As7262HardRegister.StatusReg);
                if ((status & (byte) As7262HardRegister.TxValid) == 0) break;
            }

            Device.Write(new[] { (byte) As7262HardRegister.WriteReg, (byte) (address | 0x80), (byte)1 });

            while (true)
            {
                // spinlock until we can tx data
                var status = ReadByte((byte)As7262HardRegister.StatusReg);
                if ((status & (byte)As7262HardRegister.TxValid) == 0) break;
            }

            Device.Write(new[] { (byte)As7262HardRegister.WriteReg, value, (byte)1 });
        }

        private Spectrum6 ReadSpectralData()
        {
            return new Spectrum6
            {
                Violet = ReadCalibratedChannel(As7262ChannelType.VioletCalibrated),
                Blue = ReadCalibratedChannel(As7262ChannelType.BlueCalibrated),
                Green = ReadCalibratedChannel(As7262ChannelType.GreenCalibrated),
                Yellow = ReadCalibratedChannel(As7262ChannelType.YellowCalibrated),
                Orange = ReadCalibratedChannel(As7262ChannelType.OrangeCalibrated),
                Red = ReadCalibratedChannel(As7262ChannelType.RedCalibrated)
            };
        }

        public Spectrum6 Spectrum => ReadSpectralData();
    }
}
