using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace ArcDataCore.Models.Sensor
{
    /// <summary>
    /// Represents the time-series data for a sensor.
    /// </summary>
    [MessagePackObject]
    public struct SensorData
    {
        public SensorData(SensorDataType dataType, SensorModel model, byte[] data)
        {
            DataType = dataType;
            Model = model;
            Data = data;
        }

        /// <summary>
        /// The type of sensor data.
        /// </summary>
        [Key(0)]
        public SensorDataType DataType;

        /// <summary>
        /// The sensor model.
        /// </summary>
        [Key(1)]
        public SensorModel Model;

        /// <summary>
        /// Byte array containing the raw sensor data.
        /// How this is deserialised depends on the <see cref="DataType"/> property.
        /// </summary>
        [Key(2)]
        public byte[] Data;
    }
}
