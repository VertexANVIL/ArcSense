using System;
using System.Collections.Generic;
using System.Text;

namespace ArcDataCore.Transport
{
    public static class BluetoothConstants
    {
        public const string BL_DATA_SERVICE_GUID = "DF56A355-0A5A-450F-A0AA-FF1BB6C04A5A";
        public const byte BL_DATA_SERVICE_PROTOCOL = 0x0A;

        public static readonly Guid ArcServiceGuid = new Guid(BL_DATA_SERVICE_GUID);
    }
}
