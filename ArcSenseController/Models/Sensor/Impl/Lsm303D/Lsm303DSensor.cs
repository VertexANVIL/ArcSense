using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;

namespace ArcSenseController.Models.Sensor.Impl.Lsm303D
{
    /// <summary>
    /// Interface to the LSM303D accelerometer/magnetometer.
    /// </summary>
    internal class Lsm303DSensor : I2CSensor
    {
        internal Lsm303DAccelerometer Accelerometer { get; }
        internal Lsm303DMagnetometer Magnetometer { get; }

        private const byte LSM_303D_SLAVE_ADDRESS = 0x1D;

        internal Lsm303DSensor()
        {
            Accelerometer = new Lsm303DAccelerometer(this);
            Magnetometer = new Lsm303DMagnetometer(this);
        }

        public override async Task InitialiseAsync()
        {
            await InitI2C(LSM_303D_SLAVE_ADDRESS);

            await Accelerometer.InitialiseAsync();
            await Magnetometer.InitialiseAsync();
        }

        public override SensorModel Model => SensorModel.Lsm303D;
    }
}
