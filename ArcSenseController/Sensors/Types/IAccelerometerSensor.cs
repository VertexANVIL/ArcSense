using ArcDataCore.Models.Data;

namespace ArcSenseController.Sensors.Types
{
    internal interface IAccelerometerSensor
    {
        double Frequency { get; }

        /// <summary>
        /// Reads the raw, uncompensated accelerometer data.
        /// </summary>
        AxisData3<short> Acceleration { get; }
    }
}
