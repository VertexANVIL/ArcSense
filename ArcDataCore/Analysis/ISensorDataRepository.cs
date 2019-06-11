using System.Collections.Generic;
using ArcDataCore.Models.Sensor;

namespace ArcDataCore.Analysis
{
    /// <summary>
    /// The sensor data repository's purpose is to provide an interface
    /// for data consumers to easily consume data currently in the time series database,
    /// and coming into the database via the Data Ingest Processor.
    /// </summary>
    public interface ISensorDataRepository
    {
        /// <summary>
        /// Gets the most recent sensor data ingested by the processor.
        /// </summary>
        IReadOnlyCollection<SensorData> Latest { get; }
    }
}
