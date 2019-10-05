using System;
using System.Threading.Tasks;
using System.Diagnostics;
using ArcDataCore.Models.Sensor;

namespace ArcSenseController.Sensors
{
    /// <summary>
    /// Represents a hardware sensor driver.
    /// </summary>
    internal abstract class HardwareSensorBase : ISensor
    {
        public async Task InitialiseAsync() {
            try {
                await InitialiseInternalAsync();
                Initialised = true;
            } catch (Exception e) {
                // write something to the logs about not being able to initialise this sensor, and why
                Debug.WriteLine($"Failed to initialise sensor {Model}: \"{e.Message}\"");
            }
        }

        protected abstract Task InitialiseInternalAsync();

        public abstract SensorModel Model { get; }
        public bool Initialised { get; set; }
    }
}
