using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Source;
using ArcDataCore.Transport;

namespace ArcDataCore
{
    public class DataSourceService : IDataSourceService
    {
        private const int QUEUE_CAPACITY = 250;
        private const int QUEUE_TIMEOUT = 2000;
        private readonly BlockingCollection<SensorData> _queue = new BlockingCollection<SensorData>(QUEUE_CAPACITY);

        public void RegisterTransport(IDataTransport transport)
        {
            throw new NotImplementedException();
        }

        public async Task Enqueue(SensorData data)
        {
            
            if (!_queue.TryAdd(data))
            {
                // If the queue is already full, add it to the TLDR (transitional local data repository)
                // await Tldr.Enqueue(data);

                // TODO: log something about the queue being full
            }
        }
    }
}
