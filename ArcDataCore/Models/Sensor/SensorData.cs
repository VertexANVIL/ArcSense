using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace ArcDataCore.Models.Sensor
{
    /// <summary>
    /// Represents the time-series data for a sensor at a point in time.
    /// </summary>
    [MessagePackObject]
    public struct SensorData
    {
        /// <summary>
        /// The sensor model.
        /// </summary>
        [Key(0)]
        public SensorModel Model;

        /// <summary>
        /// The date at which the data was generated.
        /// </summary>
        [Key(1)]
        public DateTime Time;

        /// <summary>
        /// The sensor data.
        /// </summary>
        [Key(2)]
        public object Data;
    }
}
