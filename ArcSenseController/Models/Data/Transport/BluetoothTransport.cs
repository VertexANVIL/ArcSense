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
using ArcDataCore.Source;
using MessagePack;

namespace ArcSenseController.Models.Data.Transport
{
    internal class BluetoothTransport : IDataTransport
    {
        private RfcommServiceProvider _uploadService;
        private StreamSocketListener _uploadListener;
        private StreamSocket _socket;
        private DataWriter _socketWriter;

        private bool _initialised;

        internal async Task<bool> Initialise()
        {
            if (_initialised) throw new Exception("The transport has already been initialised.");

            var id = RfcommServiceId.FromUuid(TransportConstants.ArcServiceGuid);
            _uploadService = await RfcommServiceProvider.CreateAsync(id);

            // Set up the SDP attributes
            InitialiseSdpAttributes(_uploadService);

            // Start advertising
            await SetAdvertise(true);

            _initialised = true;
            return true;
        }

        private void InitialiseSdpAttributes(RfcommServiceProvider provider)
        {
            var writer = new DataWriter();

            writer.WriteByte(TransportConstants.BL_SERVICE_NAME_ATTRIBUTE_TYPE);
            writer.WriteByte((byte)TransportConstants.BL_SERVICE_NAME.Length);

            writer.UnicodeEncoding = UnicodeEncoding.Utf8;
            writer.WriteString(TransportConstants.BL_SERVICE_NAME);

            provider.SdpRawAttributes.Add(TransportConstants.BL_SERVICE_NAME_ATTRIBUTE_ID, writer.DetachBuffer());
        }

        private async void UploadListenerOnConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            // If the old socket is still open, terminate it.
            await DisconnectAsync();

            _socket = args.Socket;
            _socketWriter = new DataWriter(_socket.OutputStream) {ByteOrder = ByteOrder.LittleEndian};
            Enabled = true;
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
                _socket = null;
                _socketWriter = null;
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
            if (_socket == null) return;

            await _socket.CancelIOAsync();
            _socket.Dispose();
            _socketWriter.Dispose();

            Enabled = false;
        }

        public bool Enabled { get; private set; }
        public async Task<bool> PushAsync(SensorDataPackage series, CancellationToken? token = null)
        {
            var data = MessagePackSerializer.Serialize(series);

            // Write the packet version
            _socketWriter.WriteByte(0x1A);

            // Write the length
            _socketWriter.WriteUInt32((uint)data.Length);
            _socketWriter.WriteBytes(data);

            try
            {
                var wbytes = await _socketWriter.StoreAsync();
                if (wbytes != 0) return true;
            }
            catch (Exception e)
            {
                // Socket is fucked. Disconnect.
                await DisconnectAsync();
                Debug.WriteLine($"Caught exception from socket while writing: {e.Message}");
                return false;
            }

            return false;
        }
    }
}
