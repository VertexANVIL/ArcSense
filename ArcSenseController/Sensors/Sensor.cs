using System;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcSenseController.Sensors.Types;

namespace ArcSenseController.Sensors
{
    internal abstract class Sensor : ISensor
    {
        public async Task InitialiseAsync() {
            try {
                await InitialiseInternalAsync();
                Initialised = true;
            } catch (Exception e) {
                // write something to the logs about not being able to initialise this sensor, and why
            }
        }

        protected abstract Task InitialiseInternalAsync();

        public abstract SensorModel Model { get; }
        public bool Initialised { get; set; }
    }
}
