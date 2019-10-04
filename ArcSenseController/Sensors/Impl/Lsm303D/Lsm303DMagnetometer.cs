using System.Threading.Tasks;
using ArcDataCore.Models.Data;
using ArcSenseController.Sensors.Types;

namespace ArcSenseController.Sensors.Impl.Lsm303D
{
    internal sealed class Lsm303DMagnetometer : SubSensor<Lsm303DSensor>, IMagnetometerSensor, ITemperatureSensor
    {
        internal Lsm303DMagnetometerDataRate DataRate = Lsm303DMagnetometerDataRate.Rate100Hz;
        internal Lsm303DMagnetometerMode Mode = Lsm303DMagnetometerMode.ContinuousConversion;
        internal Lsm303DMagnetometerResolution Resolution = Lsm303DMagnetometerResolution.High;
        internal Lsm303DMagnetometerScale Scale = Lsm303DMagnetometerScale.Scale12G;

        internal bool EnableTemperatureSensor = true;

        internal Lsm303DMagnetometer(Lsm303DSensor driver) : base(driver) { }

        /// <summary>
        /// Initialises the sensor.
        /// Should only be called once from the core class's Initialise method.
        /// </summary>
        protected override Task InitialiseInternalAsync()
        {
            WriteCtrlRegisters();
            return Task.CompletedTask;
        }

        private void WriteCtrlRegisters()
        {
            var reg5 = (EnableTemperatureSensor ? 0x8 : 0x00) | ((byte) Resolution << 6) | ((byte) DataRate << 2);
            Driver.Device.Write((byte)Lsm303DRegisters.Ctrl5, (byte)reg5);
            Driver.Device.Write((byte)Lsm303DRegisters.Ctrl6, (byte)((byte)Scale << 5));
            Driver.Device.Write((byte)Lsm303DRegisters.Ctrl7, (byte)Mode);
        }

        private AxisData3<short> ReadFlux()
        {
            // 0x80h = auto-increment
            var data = new byte[6];
            Driver.Device.Read((byte)((byte)Lsm303DRegisters.OutXla | 0x80), data);

            return new AxisData3<short>(
                (short)(data[0] | (data[1] << 8)),
                (short)(data[2] | (data[3] << 8)),
                (short)(data[4] | (data[5] << 8)));
        }

        private double ReadTemperature()
        {
            // Reads two's compliment right justified value.
            var data = new byte[2];
            Driver.Device.Read((byte)((byte)Lsm303DRegisters.TempOutL | 0x80), data);

            return (data[1] << 8 | data[0]) / 8 + 25;
        }

        public double Temperature => ReadTemperature();
        public AxisData3<short> Flux => ReadFlux();
    }
}
