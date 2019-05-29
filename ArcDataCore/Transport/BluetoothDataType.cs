using System;
using System.Collections.Generic;
using System.Text;

namespace ArcDataCore.Transport
{
    public enum BluetoothDataType : byte
    {
        Command = 0x10,
        SensorData = 0x20,
    }
}
