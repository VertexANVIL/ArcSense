using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using ArcDataCore.Models.Sensor;
using MessagePack;

namespace ArcView.Services
{
    internal class BluetoothClient
    {
        private readonly IBluetoothService _service;
        private BinaryReader _reader;
        private BinaryWriter _writer;
        private Thread _thread;

        internal BluetoothClient(IBluetoothService service)
        {
            _service = service;
            _thread = new Thread(Recieve);

            // Register event handlers
            _service.Connected += ServiceOnConnected;
            _service.Disconnected += ServiceOnDisconnected;
        }

        private void ServiceOnConnected()
        {
            _reader = new BinaryReader(_service.InputStream);
            _writer = new BinaryWriter(_service.OutputStream);
            _thread.Start();
        }

        private void ServiceOnDisconnected()
        {
            _reader = null;
            _writer = null;
            _thread.Abort();
        }

        private void Recieve()
        {
            try
            {
                // Ensure protocol version matches
                var header = _reader.ReadByte();
                if (header != 0x1A) throw new Exception($"Unsupported protocol version {header}!");

                var length = _reader.ReadUInt32();
                var data = _reader.ReadBytes((int)length);

                var obj = MessagePackSerializer.Deserialize<SensorDataPackage>(data);
                // TODO: do something with package

                Debug.WriteLine($"Read package @ {obj.TimeStamp} with {obj.Data.Length} sensor datapoints");
            }
            catch (Exception e)
            {
                // Caught an exception so assume the socket is broken
                _service.Disconnect();
            }
        }
    }
}
