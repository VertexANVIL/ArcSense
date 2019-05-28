using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Source;

namespace ArcDataCore.Transport
{
    /// <summary>
    /// Represents a method of uploading data to a source, aka, a transport.
    /// </summary>
    public interface IDataTransport
    {
        /// <summary>
        /// Whether or not the transport is enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Pushes a data point to the transport's stream.
        /// </summary>
        /// <param name="series">The data point.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>True if the push was successful, otherwise false.</returns>
        Task<bool> PushAsync(SensorDataPackage series, CancellationToken? token = null);
    }
}
