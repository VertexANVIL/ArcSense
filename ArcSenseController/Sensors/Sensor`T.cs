using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcSenseController.Sensors.Types;

namespace ArcSenseController.Sensors
{
    /// <summary>
    /// Represents a sensor that is a subset of a master driver.
    /// </summary>
    /// <typeparam name="T">The type of the driver.</typeparam>
    internal abstract class SubSensor<T> : Sensor where T: ISensor
    {
        protected readonly T Driver;
        internal SubSensor(T driver)
        {
            Driver = driver;
        }

        protected override Task InitialiseInternalAsync() => Task.CompletedTask;

        public override SensorModel Model => Driver.Model;
    }
}
