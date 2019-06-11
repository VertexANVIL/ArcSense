using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Transport;
using ArcDataCore.TxRx;
using MessagePack;
using MessagePack.Resolvers;

namespace ArcView.Services
{
    internal class BluetoothReciever : IReciever
    {
        private const int STACK_CAPACITY = 10;

        private readonly IBluetoothService _service;
        private BinaryReader _reader;
        private BinaryWriter _writer;
        private Thread _thread;

        /// <summary>
        /// Stack that holds incoming data packages before they are processed.
        /// </summary>
        private readonly BlockingCollection<TransportDataPackage> _pushStack 
            = new BlockingCollection<TransportDataPackage>(new ConcurrentStack<TransportDataPackage>(), STACK_CAPACITY);

        private bool _recieve;

        public BluetoothReciever(IBluetoothService service)
        {
            _service = service;

            // Register event handlers
            _service.Connected += ServiceOnConnected;
            _service.Disconnected += ServiceOnDisconnected;
        }

        private void ServiceOnConnected()
        {
            _reader = new BinaryReader(_service.InputStream);
            _writer = new BinaryWriter(_service.OutputStream);
            _recieve = true;

            // if the thread is still running (somehow), terminate it now
            if (_thread != null && _thread.IsAlive) _thread.Abort();

            _thread = new Thread(Recieve);
            _thread.Start();
        }

        private void ServiceOnDisconnected()
        {
            _reader = null;
            _writer = null;
            _recieve = false;
        }

        private void Recieve()
        {
            try
            {
                while (_recieve)
                {
                    // Ensure protocol version matches
                    var header = _reader.ReadByte();
                    if (header != BluetoothConstants.BL_DATA_SERVICE_PROTOCOL)
                        throw new Exception($"Unsupported protocol version {header}!");

                    var type = (BluetoothDataType)_reader.ReadByte();
                    switch (type)
                    {
                        case BluetoothDataType.Command:
                            // TODO: read whether we care about a response
                            var respond = _reader.ReadBoolean();

                            break;
                        case BluetoothDataType.SensorData:
                            var length = _reader.ReadUInt32();
                            var data = _reader.ReadBytes((int)length);

                            var obj = MessagePackSerializer.Deserialize<TransportDataPackage>(data);
                            Debug.WriteLine($"BT: Read package @ {obj.TimeStamp}");
                            if (!_pushStack.TryAdd(obj)) Debug.WriteLine($"BT: Ignoring @ {obj.TimeStamp} (Package stack is full!)");

                            break;
                        default:
                            throw new Exception($"Unsupported bluetooth data type {type}!");
                    }
                }
            }
            catch (Exception e)
            {
                // Caught an exception so assume the socket is broken
                _service.Disconnect();
            }
        }

        public Task<TransportDataPackage> PullAsync(CancellationToken? token)
        {
            return token != null 
                ? Task.FromResult(_pushStack.Take(token.Value)) 
                : Task.FromResult(_pushStack.Take());
        }
    }
}
