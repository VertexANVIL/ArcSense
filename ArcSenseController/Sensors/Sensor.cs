using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcSenseController.Sensors.Types;

namespace ArcSenseController.Sensors
{
    internal abstract class Sensor : ISensor
    {
        public abstract Task InitialiseAsync();

        public abstract SensorModel Model { get; }
    }
}
