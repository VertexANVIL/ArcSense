using System;
using MessagePack;

namespace ArcDataCore.Transport
{
    /// <summary>
    /// Represents multiple <see cref="TransportData"/> instances grouped by a <see cref="DateTime"/>.
    /// </summary>
    [MessagePackObject]
    public class TransportDataPackage
    {
        public TransportDataPackage(DateTime timeStamp, TransportData[] data)
        {
            TimeStamp = timeStamp;
            Data = data;
        }

        [Key(0)]
        public readonly DateTime TimeStamp;

        [Key(1)]
        public readonly TransportData[] Data;
    }
}
