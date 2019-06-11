using System.Threading;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Transport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using ArcDataCore.TxRx;
using ArcSenseController.Services;
using MessagePack;
using MessagePack.Resolvers;

namespace ArcSenseController.Models.Data.Transport
{
    internal class BluetoothTransport : ITransmitter
    {
        private readonly BluetoothService _service;

        public BluetoothTransport(BluetoothService service)
        {
            _service = service;
        }

        public bool Enabled => _service.Connected;

        public async Task<bool> PushAsync(TransportDataPackage series)
        {
            var data = MessagePackSerializer.Serialize(series);
            using (var context = await _service.GetContextAsync())
            {
                var writer = context.Writer;

                // Write the protocol version and packet type
                writer.WriteByte(BluetoothConstants.BL_DATA_SERVICE_PROTOCOL);
                writer.WriteByte((byte)BluetoothDataType.SensorData);

                // Write the length
                writer.WriteUInt32((uint)data.Length);
                writer.WriteBytes(data);

                // Try to send
                return await context.StoreAsync();
            }
        }
    }
}
