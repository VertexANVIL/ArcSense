using System.Collections.Generic;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcSenseController.Sensors.Types;
using ArcSenseController.Services;

namespace ArcSenseController.Sensors.Impl.Lsm303D
{
    /// <summary>
    /// Interface to the LSM303D accelerometer/magnetometer.
    /// </summary>
    internal sealed class Lsm303DSensor : I2CSensor, ISplitSensor
    {
        internal Lsm303DAccelerometer Accelerometer { get; }
        internal Lsm303DMagnetometer Magnetometer { get; }

        private const byte LSM_303D_SLAVE_ADDRESS = 0x1D;

        public Lsm303DSensor(I2CService service) : base(service)
        {
            Accelerometer = new Lsm303DAccelerometer(this);
            Magnetometer = new Lsm303DMagnetometer(this);
        }

        public override async Task InitialiseAsync()
        {
            InitI2C(LSM_303D_SLAVE_ADDRESS);

            await Accelerometer.InitialiseAsync();
            await Magnetometer.InitialiseAsync();
        }

        public override SensorModel Model => SensorModel.Lsm303D;
        public IEnumerable<ISensor> SubSensors => new ISensor[] { Accelerometer, Magnetometer };
    }
}
