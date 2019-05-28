using System;
using System.Collections.Generic;
using System.Text;

namespace ArcDataCore.Transport
{
    public static class TransportConstants
    {
        public const string BL_SERVICE_GUID = "DF56A355-0A5A-450F-A0AA-FF1BB6C04A5A";
        public const uint BL_SERVICE_VERSION_ATTRIBUTE_ID = 0x0300;
        public const byte BL_SERVICE_VERSION_ATTRIBUTE_TYPE = 0x0A;
        public const uint BL_SERVICE_VERSION = 100;

        public static readonly Guid ArcServiceGuid = new Guid(BL_SERVICE_GUID);
        public const ushort BL_SERVICE_NAME_ATTRIBUTE_ID = 0x100;
        public const byte BL_SERVICE_NAME_ATTRIBUTE_TYPE = (4 << 3) | 5; // attribute type size, sdp attribute type value
        public const string BL_SERVICE_NAME = "ArcSense Stream Service";
    }
}
