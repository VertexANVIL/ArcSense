using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ArcView.Services
{
    public interface IBluetoothService
    {
        bool IsConnected { get; }
        Stream OutputStream { get; }
        Stream InputStream { get; }
        string DeviceAddress { get; }

        Task<bool> TryConnectAsync();
        void Disconnect();

        event Action Connected;
        event Action Disconnected;
    }
}
