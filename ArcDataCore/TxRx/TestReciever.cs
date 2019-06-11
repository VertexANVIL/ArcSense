using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArcDataCore.Models.Data;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Transport;
using MessagePack;

namespace ArcDataCore.TxRx
{
    /// <summary>
    /// The purpose of this class is to generate random sensor package data
    /// for testing of various data models and processors.
    /// </summary>
    public class TestReciever : IReciever
    {
        private readonly Random _random = new Random();

        public async Task<TransportDataPackage> PullAsync(CancellationToken? token)
        {
            // Introduce artificial delay of 1 second
            await Task.Delay(TimeSpan.FromSeconds(1));

            return RandomPackage();
        }

        // This method is very similar to the one in SensorDataAdapter,
        // it shares a lot of the same semantics but just deals with test data.
        private TransportDataPackage RandomPackage()
        {
            // List to store test data
            var dest = new List<TransportData>();

            // these random readings are taken from ambient in my room
            // TODO: get some more readings from other places and build profiles

            foreach (var type in (SensorDataType[]) Enum.GetValues(typeof(SensorDataType)))
            {
                object data;

                if (type == SensorDataType.Unknown) continue;

                switch (type)
                {
                    case SensorDataType.Accelerometer3D:
                        data = new AxisData3<short>((short)_random.Next(-1000, 1000), (short)_random.Next(-1000, 1000), (short)_random.Next(-1000, 1000));
                        break;
                    case SensorDataType.Magnetometer3D:
                        data = new AxisData3<short>((short)_random.Next(-1000, 1000), (short)_random.Next(-1000, 1000), (short)_random.Next(-1000, 1000));
                        break;
                    case SensorDataType.GasResistance:
                        data = _random.Next(116560, 116590) + _random.NextDouble();
                        break;
                    case SensorDataType.RelativeHumidity:
                        data = 44 + _random.NextDouble();
                        break;
                    case SensorDataType.Pressure:
                        data = _random.Next(100000, 102000) + _random.NextDouble();
                        break;
                    case SensorDataType.Temperature:
                        data = _random.Next(25, 26) + _random.NextDouble();
                        break;
                    case SensorDataType.Spectral:
                        continue;
                        //data = new byte[] {64, 15, 131, 78, 207, 52, 130, 78, 225, 34, 132, 78, 109, 118, 131, 78, 199, 2, 133, 78, 118, 232, 131, 78};
                        break;
                    case SensorDataType.Colour:
                        continue;
                        data = null;
                        break;
                    case SensorDataType.Radiation:
                        data = _random.Next(18, 25);
                        break;
                    default:
                        continue;
                }

                var array = MessagePackSerializer.Serialize(data);
                dest.Add(new TransportData(type, SensorModel.Unknown, array));
            }

            return new TransportDataPackage(DateTime.UtcNow, dest.ToArray());
        }
    }
}
