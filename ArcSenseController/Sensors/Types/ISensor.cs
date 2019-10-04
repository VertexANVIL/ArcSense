using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;

namespace ArcSenseController.Sensors.Types
{
    /// <summary>
    /// Base sensor interface.
    /// </summary>
    internal interface ISensor
    {
        bool Initialised {get; set;}

        /// <summary>
        /// Initialises the sensor.
        /// </summary>
        Task InitialiseAsync();

        /// <summary>
        /// Gets the name of the underlying sensor device.
        /// </summary>
        SensorModel Model { get; }
    }
}
