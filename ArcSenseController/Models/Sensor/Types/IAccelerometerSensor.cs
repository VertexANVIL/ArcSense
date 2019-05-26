using System.Numerics;

namespace ArcSenseController.Models.Sensor.Types
{
    internal interface IAccelerometerSensor
    {
        double Frequency { get; }

        /// <summary>
        /// Reads the raw, uncompensated accelerometer data.
        /// </summary>
        byte[] Acceleration { get; }
    }
}
