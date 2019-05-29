using System.Collections.Concurrent;
using System.Threading;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Transport;

namespace ArcDataCore.TxRx
{
    public class TransmissionService : ITransmissionService
    {
        private const int STACK_CAPACITY = 10;
        private const int UPLOAD_TIMEOUT = 1000; // Upload timeout of 1 second

        private readonly Thread _uploader;
        //private readonly CancellationTokenSource _cts = new CancellationTokenSource(UPLOAD_TIMEOUT);

        /// <summary>
        /// This stack represents a persistent, database-backed local stack, or a dummy in-memory stack.
        /// It is enqueued to by the Collector thread and dequeued by the Uploader thread.
        /// </summary>
        private readonly BlockingCollection<SensorDataPackage> _stack 
            = new BlockingCollection<SensorDataPackage>(new ConcurrentStack<SensorDataPackage>(), STACK_CAPACITY); // TODO: make this fs-backed

        /// <summary>
        /// The transport used by the service.
        /// </summary>
        public ITransmitter Transport { private get; set; } = new DebugTransmitter();

        public TransmissionService()
        {
            _uploader = new Thread(async () =>
            {
                do
                {
                    if (!Transport.Enabled) continue;

                    // TODO: peek instead so we can abort...
                    if (!_stack.TryTake(out var item)) continue;
                    //var token = _cts.Token;

                    // Try to push the item onto the transport.
                    if (!await Transport.PushAsync(item))
                    {
                        // TODO: push item back onto the stack
                        _stack.Add(item);
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
