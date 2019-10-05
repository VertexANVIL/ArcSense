using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;

namespace ArcSenseController.Sensors
{
    /// <summary>
    /// Represents a sensor that is a subset of a driver.
    /// </summary>
    /// <typeparam name="T">The type of the driver.</typeparam>
    internal abstract class HardwareSensor<T> : HardwareSensorBase where T: ISensor
    {
        protected readonly T Driver;
        internal HardwareSensor(T driver)
        {
            Driver = driver;
        }

        protected override Task InitialiseInternalAsync() => Task.CompletedTask;

        public override SensorModel Model => Driver.Model;
    }
}
