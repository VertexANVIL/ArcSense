using ArcDataCore.Models.Sensor;
using ArcDataCore.Transport;

namespace ArcDataCore.TxRx
{
    public interface ITransmissionService
    {
        ITransmitter Transport { set; }

        void Commit(TransportDataPackage package);

        /// <summary>
        /// Enables the uploader thread.
        /// </summary>
        void Start();

        /// <summary>
        /// Disables the uploader thread.
        /// </summary>
        void Stop();
    }
}
