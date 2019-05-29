using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Transport;
using MessagePack;

namespace ArcSenseController.Services
{
    internal class BluetoothService
    {
        private RfcommServiceProvider _uploadService;
        private StreamSocketListener _uploadListener;
        private StreamSocket _socket;
        private DataReader _reader;
        private DataWriter _writer;

        private bool _initialised;
        private BluetoothContext _context;
        private SemaphoreSlim _semaphore 
            = new SemaphoreSlim(1);

        private class BluetoothContext : IBluetoothContext
        {
            public DataReader Reader { get; internal set; }
            public DataWriter Writer { get; internal set; }

            private readonly BluetoothService _service;

            internal BluetoothContext(BluetoothService service)
            {
                _service = service;
            }

            public async Task<bool> StoreAsync() => await _service.StoreAsync();

            /// <summary>
            /// Disposes the context.
            /// This must be called to release the lock.
            /// </summary>
            public void Dispose()
            {
                _service.CheckinContext();
            }
        }

        internal interface IBluetoothContext : IDisposable
        {
            DataReader Reader { get; }
            DataWriter Writer { get; }

            /// <summary>
            /// Stores the data in the write buffer and flushes the stream.
            /// </summary>
            /// <returns>Whether the store was successful.</returns>
            Task<bool> StoreAsync();
        }

        /// <summary>
        /// Gets a context that can be used to access the Bluetooth socket in a thread-safe way.
        /// </summary>
        internal async Task<IBluetoothContext> GetContextAsync()
        {
            await _semaphore.WaitAsync();
            _context = new BluetoothContext(this) { Reader = _reader, Writer = _writer };

            return _context;
        }

        private async void CheckinContext()
        {
            // Flush the writer, in case
            // someone forgot to send all bytes...
            if (Connected)
                await _writer.FlushAsync();

            _semaphore.Release();
        }

        internal async Task<bool> Initialise()
        {
            if (_initialised) throw new Exception("The service has already been initialised.");

            var id = RfcommServiceId.FromUuid(BluetoothConstants.ArcServiceGuid);
            _uploadService = await RfcommServiceProvider.CreateAsync(id);

            // Start advertising
            await SetAdvertise(true);

            _initialised = true;
            return true;
        }

        private async void UploadListenerOnConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            // If the old socket is still open, terminate it.
            await DisconnectAsync();

            _socket = args.Socket;
            _reader = new DataReader(_socket.InputStream) { ByteOrder = ByteOrder.LittleEndian };
            _writer = new DataWriter(_socket.OutputStream) { ByteOrder = ByteOrder.LittleEndian };
            Connected = true;
        }

        /// <summary>
        /// Enables or disables advertising the bluetooth service.
        /// </summary>
        /// <param name="advertise">True to enable, False to disable.</param>
        public async Task SetAdvertise(bool advertise)
        {
            if (!advertise)
            {
                // Clean up the listening objects
                _uploadService.StopAdvertising();
                _uploadListener = null;
                return;
            }

            _uploadListener = new StreamSocketListener();
            _uploadListener.ConnectionReceived += UploadListenerOnConnectionReceived;

            await _uploadListener.BindServiceNameAsync(_uploadService.ServiceId.AsString(),
                SocketProtectionLevel.BluetoothEncryptionWithAuthentication);

            _uploadService.StartAdvertising(_uploadListener);
        }

        /// <summary>
        /// Closes the socket and disables the transport.
        /// </summary>
        private async Task DisconnectAsync()
        {
            if (!Connected) return;

            await _socket.CancelIOAsync();
            _socket.Dispose();
            _reader.Dispose();
            _writer.Dispose();

            Connected = false;
        }

        private async Task<bool> StoreAsync()
        {
            try
            {
                var wbytes = await _writer.StoreAsync();
                return wbytes != 0;
            }
            catch (Exception e)
            {
                // Socket is fucked. Disconnect.
                await DisconnectAsync();
                Debug.WriteLine($"Caught exception from socket while writing: {e.Message}");
                return false;
            }
        }

        public bool Connected;
    }
}
