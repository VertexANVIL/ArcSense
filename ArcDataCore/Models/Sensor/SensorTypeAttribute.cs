using System;
using System.Collections.Generic;
using System.Text;

namespace ArcDataCore.Models.Sensor
{
    /// <summary>
    /// Represents the sensor with the highest priority when
    /// displaying values on reports. Lower sensors will be used for error correction only.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    internal class SensorTypeAttribute : Attribute
    {
        public readonly int Priority;

        /// <summary>
        /// The sensor data type this attribute applies to. If the value of this is <see cref="SensorDataType.Unknown"/>,
        /// the priority will apply to all sensor data types this sensor reports.
        /// </summary>
        public readonly SensorDataType? DataType;

        internal SensorTypeAttribute(int priority, SensorDataType dataType = SensorDataType.Unknown)
        {
            Priority = priority;
            DataType = dataType;
        }

        internal SensorTypeAttribute(SensorPriority priority, SensorDataType dataType = SensorDataType.Unknown)
        {
            Priority = (int) priority;
            DataType = dataType;
        }
    }
}
