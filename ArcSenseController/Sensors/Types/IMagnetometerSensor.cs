using ArcDataCore.Models.Data;

namespace ArcSenseController.Sensors.Types
{
    internal interface IMagnetometerSensor
    {
        /// <summary>
        /// Returns the raw 3-axis magnetometer data (non-compensated).
        /// </summary>
        AxisData3<short> Flux { get; }
    }
}
