using System;
using System.Collections.Generic;
using System.Text;

namespace ArcDataCore.Models.Sensor
{
    public class SensorData
    {
        public SensorData(SensorDataType type, SensorModel model, object data)
        {
            DataType = type;
            Model = model;
            Data = data;
        }

        /// <summary>
        /// The type of sensor data.
        /// </summary>
        public readonly SensorDataType DataType;

        /// <summary>
        /// The sensor model.
        /// </summary>
        public readonly SensorModel Model;

        /// <summary>
        /// Object representing the sensor data model.
        /// </summary>
        public readonly object Data;
    }
}
