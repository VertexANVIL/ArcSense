using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Transport;

namespace ArcDataCore.Source
{
    public class DataSourceService : IDataSourceService
    {
        private const int QUEUE_CAPACITY = 250;
        private const int UPLOAD_TIMEOUT = 1000; // Upload timeout of 1 second

        private readonly Thread _uploader;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource(UPLOAD_TIMEOUT);

        /// <summary>
        /// This stack represents a persistent, database-backed local stack, or a dummy in-memory stack.
        /// It is enqueued to by the Collector thread and dequeued by the Uploader thread.
        /// </summary>
        private readonly BlockingCollection<SensorDataPackage> _stack 
            = new BlockingCollection<SensorDataPackage>(new ConcurrentStack<SensorDataPackage>(), QUEUE_CAPACITY); // TODO: make this fs-backed

        /// <summary>
        /// The transport used by the service.
        /// </summary>
        public IDataTransport Transport { private get; set; } = new DebugTransport();

        public DataSourceService()
        {
            _uploader = new Thread(async () =>
            {
                do
                {
                    // TODO: peek instead so we can abort...
                    if (!_stack.TryTake(out var item)) continue;
                    var token = _cts.Token;

                    // TODO: what should we really do if a transport isn't enabled? probably shouldn't just discard the data...
                    if (!Transport.Enabled) continue;

                    if (!await Transport.PushAsync(item, token))
                    {
                        // TODO: put back into the queue
                    }
                } while (true);
            });
        }

        public void Commit(SensorDataPackage package)
        {
            if (!_stack.TryAdd(package))
            {
                // TODO: even this queue is full. Log something and return.
                //Debug.WriteLine("Tried to add something to the queue, but it's already full!");
                return;
            }
        }

        public void Start() => _uploader.Start();
        public void Stop() => _uploader.Abort();
    }
}
