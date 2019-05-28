using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace ArcDataCore.Models.Sensor
{
    /// <summary>
    /// Represents multiple <see cref="SensorData"/> instances grouped by a <see cref="DateTime"/>.
    /// </summary>
    [MessagePackObject]
    public struct SensorDataPackage
    {
        public SensorDataPackage(DateTime timeStamp, SensorData[] data)
        {
            TimeStamp = timeStamp;
            Data = data;
        }

        [Key(0)]
        public readonly DateTime TimeStamp;

        [Key(1)]
        public readonly SensorData[] Data;
    }
}
