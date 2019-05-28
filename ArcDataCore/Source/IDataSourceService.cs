using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Transport;

namespace ArcDataCore.Source
{
    public interface IDataSourceService
    {
        IDataTransport Transport { set; }

        void Commit(SensorDataPackage package);

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
