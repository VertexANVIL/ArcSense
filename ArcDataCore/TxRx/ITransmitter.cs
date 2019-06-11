using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Transport;

namespace ArcDataCore.TxRx
{
    /// <summary>
    /// Represents a method of uploading data to a source, aka, a transport.
    /// </summary>
    public interface ITransmitter
    {
        /// <summary>
        /// Whether or not the transmitter is enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Pushes a data point to the transport's stream.
        /// </summary>
        /// <param name="series">The data point.</param>
        /// <returns>True if the push was successful, otherwise false.</returns>
        Task<bool> PushAsync(TransportDataPackage series);
    }
}
