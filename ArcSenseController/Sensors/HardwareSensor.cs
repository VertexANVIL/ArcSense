using System;
using System.Collections.Generic;
using ArcDataCore.Models.Sensor;

namespace ArcSenseController.Sensors
{
    internal abstract class HardwareSensor : HardwareSensorBase
    {
        /// <summary>
        /// Reads the sensor data.
        /// </summary>
        public abstract (SensorDataType, object)[] Read();
    }
}