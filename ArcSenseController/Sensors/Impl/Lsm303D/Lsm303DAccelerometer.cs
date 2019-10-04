using System.Threading.Tasks;
using ArcDataCore.Models.Data;
using ArcSenseController.Sensors.Types;

namespace ArcSenseController.Sensors.Impl.Lsm303D
{
    internal sealed class Lsm303DAccelerometer : SubSensor<Lsm303DSensor>, IAccelerometerSensor
    {
        internal Lsm303DAccelerometerDataRate DataRate = Lsm303DAccelerometerDataRate.Rate400Hz;
        internal Lsm303DAccelerometerFilterBandwidth Bandwidth = Lsm303DAccelerometerFilterBandwidth.Bandwidth194Hz;
        internal Lsm303DAccelerometerScale Scale = Lsm303DAccelerometerScale.Scale4G;

        internal bool EnableXAxis = true;
        internal bool EnableYAxis = true;
        internal bool EnableZAxis = true;
        internal Lsm303DAccelerometer(Lsm303DSensor driver) : base(driver) { }

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
            var reg1 = ((byte) DataRate << 4) | 
                        (EnableZAxis ? 0x1 << 2 : 0x0) | 
                        (EnableYAxis ? 0x1 << 1 : 0x0) |
                        (EnableXAxis ? 0x1 : 0x0);
            Driver.Device.Write((byte)Lsm303DRegisters.Ctrl1, (byte)reg1);

            var reg2 = ((byte)Bandwidth << 6) | ((byte)Scale << 3);
            Driver.Device.Write((byte)Lsm303DRegisters.Ctrl2, (byte)reg2);
        }

        private AxisData3<short> ReadAcceleration()
        {
            var data = new byte[6];
            Driver.Device.Read((byte)((byte)Lsm303DRegisters.OutXlm | 0x80), data);

            return new AxisData3<short>(
                (short) (data[0] | (data[1] << 8)),
                (short) (data[2] | (data[3] << 8)),
                (short) (data[4] | (data[5] << 8)));
        }

        public double Frequency => 400d;
        public AxisData3<short> Acceleration => ReadAcceleration();
    }
}
