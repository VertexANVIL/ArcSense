using System.Numerics;

namespace ArcSenseController.Models.Sensor.Types
{
    internal interface IMagnetometerSensor
    {
        /// <summary>
        /// Returns the raw 3-axis magnetometer data (non-compensated).
        /// </summary>
        byte[] Flux { get; }
    }
}
