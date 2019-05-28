using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ArcDataCore.Transport;
using ArcView.Services;
using Java.Util;

namespace ArcView.Droid.Services
{
    internal class BluetoothService : IBluetoothService
    {
        public bool IsConnected => _socket != null && _socket.IsConnected;
        public Stream OutputStream => _socket?.OutputStream;
        public Stream InputStream => _socket?.InputStream;

        public string DeviceAddress { get; private set; }

        private BluetoothSocket _socket;
        private readonly BluetoothAdapter _adapter;

        public event Action Connected;
        public event Action Disconnected;

        public BluetoothService()
        {
            _adapter = BluetoothAdapter.DefaultAdapter;
        }

        public async Task<bool> TryConnectAsync()
        {
            if (!_adapter.IsEnabled) return false;
            if (await TryReconnectAsync())
            {
                Connected?.Invoke();
                return true;
            }

            foreach (var device in _adapter.BondedDevices)
                if (await TryConnectDeviceAsync(device))
                {
                    Connected?.Invoke();
                    return true;
                }

            // None of the devices satisfied the conditions
            return false;
        }

        /// <summary>
        /// Attempts to reconnect to the last known good device.
        /// </summary>
        public async Task<bool> TryReconnectAsync()
        {
            if (string.IsNullOrWhiteSpace(DeviceAddress)) return false;

            var device = _adapter.GetRemoteDevice(DeviceAddress);
            if (device != null && device.BondState == Bond.Bonded)
                return await TryConnectDeviceAsync(device);

            DeviceAddress = null;
            return false;
        }

        private async Task<bool> TryConnectDeviceAsync(BluetoothDevice device)
        {
            var uuid = UUID.FromString(TransportConstants.BL_SERVICE_GUID);
            var socket = device.CreateRfcommSocketToServiceRecord(uuid);
            if (socket == null) return false;

            // Connect if we're not already connected
            if (!socket.IsConnected)
                await socket.ConnectAsync();

            _socket = socket;
            DeviceAddress = device.Address;
            return IsConnected;
        }

        public void Disconnect()
        {
            if (!IsConnected) return;
            _socket.Close();
            _socket = null;

            Disconnected?.Invoke();
        }
    }
}