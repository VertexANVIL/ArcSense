using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArcView.Services
{
    /// <summary>
    /// The purpose of this service is to ensure Bluetooth stays connected when it is enabled,
    /// and tries to reconnect failed connections and show UI to indicate the connection state.
    /// </summary>
    internal class BluetoothNotifyService
    {
        private readonly IInAppNotifyService _notifier;
        private readonly IBluetoothService _bluetooth;
        private bool _connecting;
        public BluetoothNotifyService(IInAppNotifyService notifier, IBluetoothService bluetooth)
        {
            _notifier = notifier;
            _bluetooth = bluetooth;
        }

        public void TryConnect()
        {
            if (_connecting) return;

            var notification = _notifier.Make("Trying to connect...", -2);
            notification.Show();

            _connecting = true;

            Task.Run(async () =>
            {
                bool result;

                // Keep trying to connect indefinitely
                // with a pause of 5 seconds in between attempts

                do
                {
                    result = await _bluetooth.TryConnectAsync();
                    if (!result) await Task.Delay(TimeSpan.FromSeconds(5));
                } while (result == false);

                _connecting = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    notification.Dismiss();
                });
            }).ConfigureAwait(false);
        }
    }
}
