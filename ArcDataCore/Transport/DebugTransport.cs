using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using MessagePack;

namespace ArcDataCore.Transport
{
    /// <summary>
    /// Transport that logs everything that it sees to the console.
    /// Useful for debugging or development purposes.
    /// </summary>
    public class DebugTransport : IDataTransport
    {
        private static int _totalWritten;

        public bool Enabled => true;

        public Task<bool> PushAsync(SensorDataPackage series, CancellationToken? token = null)
        {
            var totalBytes = MessagePackSerializer.Serialize(series);
            _totalWritten += totalBytes.Length;

            Debug.WriteLine($"Debug data from {series.Data}: {totalBytes.Length} bytes");

            //Debug.WriteLine($"Debug data: {data.Model}, {data.DataType}");
            return Task.FromResult(true);
        }
    }
}
