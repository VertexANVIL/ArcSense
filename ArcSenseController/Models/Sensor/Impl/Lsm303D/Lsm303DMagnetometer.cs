using System.Numerics;
using System.Threading.Tasks;
using ArcSenseController.Models.Sensor.Types;

namespace ArcSenseController.Models.Sensor.Impl.Lsm303D
{
    internal class Lsm303DMagnetometer : Sensor<Lsm303DSensor>, IMagnetometerSensor, ITemperatureSensor
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
        public override Task InitialiseAsync()
        {
            WriteCtrlRegisters();
            return Task.CompletedTask;
        }

        private void WriteCtrlRegisters()
        {
            var reg5 = (EnableTemperatureSensor ? 0x8 : 0x00) | ((byte) Resolution << 6) | ((byte) DataRate << 2);
            Driver.Device.Write(new [] { (byte)Lsm303DRegisters.Ctrl5, (byte)reg5 });
            Driver.Device.Write(new [] { (byte)Lsm303DRegisters.Ctrl6, (byte)((byte)Scale << 5) });
            Driver.Device.Write(new [] { (byte)Lsm303DRegisters.Ctrl7, (byte)Mode });
        }

        private byte[] ReadRawFlux()
        {
            var data = new byte[6];
            Driver.Device.WriteRead(new[] { (byte)((byte)Lsm303DRegisters.OutXla | 0x80) }, data);

            return data;
        }

        private double ReadTemperature()
        {
            // Reads two's compliment right justified value.

            var data = new byte[2];
            Driver.Device.WriteRead(new[] { (byte)((byte)Lsm303DRegisters.TempOutL | 0x80) }, data);

            return (data[1] << 8 | data[0]) / 8 + 25;
        }

        public double Temperature => ReadTemperature();
        public byte[] Flux => ReadRawFlux();
    }
}
