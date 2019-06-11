using System;
using ArcDataCore.Models.Data;
using ArcDataCore.Models.Sensor;
using MessagePack;

namespace ArcDataCore.Transport
{
    public static class TransportDataSerialiser
    {
        public static byte[] Serialise(object data)
        {
            return MessagePackSerializer.Serialize(data);
        }

        public static object Deserialise(byte[] data, SensorDataType dataType)
        {
            Type type;

            // deserialise data custom
            switch (dataType)
            {
                case SensorDataType.Accelerometer3D:
                case SensorDataType.Magnetometer3D:
                    type = typeof(AxisData3<short>);
                    break;
                case SensorDataType.GasResistance:
                case SensorDataType.RelativeHumidity:
                case SensorDataType.Pressure:
                case SensorDataType.Temperature:
                    type = typeof(double);
                    break;
                case SensorDataType.Spectral:
                    type = typeof(Spectrum6);
                    break;
                case SensorDataType.Colour:
                    throw new NotImplementedException();
                case SensorDataType.Radiation:
                    type = typeof(int);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var result = MessagePackSerializer.NonGeneric.Deserialize(type, data);
            return result;
        }
    }
}
