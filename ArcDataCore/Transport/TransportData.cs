using ArcDataCore.Models.Sensor;
using MessagePack;

namespace ArcDataCore.Transport
{
    /// <summary>
    /// Represents the time-series data for a sensor, ready for transport "over the wire"
    /// </summary>
    [MessagePackObject]
    public struct TransportData
    {
        public TransportData(SensorDataType type, SensorModel model, byte[] data)
        {
            DataType = type;
            Model = model;
            Data = data;
        }

        /// <summary>
        /// The type of sensor data.
        /// </summary>
        [Key(0)]
        public readonly SensorDataType DataType;

        /// <summary>
        /// The sensor model.
        /// </summary>
        [Key(1)]
        public readonly SensorModel Model;

        /// <summary>
        /// Byte array containing the raw sensor data.
        /// How this is deserialised depends on the <see cref="DataType"/> property.
        /// </summary>
        [Key(2)]
        public readonly byte[] Data;
    }
}
