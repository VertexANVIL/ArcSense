using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Models.Sensor.Virtual;
using ArcDataCore.Transport;
using ArcDataCore.TxRx;
using Microsoft.Extensions.DependencyInjection;

namespace ArcDataCore.Analysis
{
    public class DataIngestProcessor
    {
        public IReciever Reciever { get; set; }
        private Thread _thread;

        public IReadOnlyCollection<SensorData> Latest { get; private set; }
        public event Action<IReadOnlyCollection<SensorData>> DataIngested;

        private readonly IReadOnlyCollection<IVirtualSensor> _virtualSensors;

        public DataIngestProcessor(IServiceProvider provider)
        {
            _virtualSensors = provider.GetServices<IVirtualSensor>().ToList();
        }

        /// <summary>
        /// Starts the processor thread.
        /// </summary>
        public void Start()
        {
            _thread = new Thread(Process);
            _thread.Start();
        }

        private async void Process()
        {
            while (true)
            {
                // TODO: try/catch here, and log
                var package = await Reciever.PullAsync(null);
                Debug.WriteLine($"DIP: Ingesting package @ {package.TimeStamp} with {package.Data.Length} sensor datapoints");

                // process and deserialise each
                var list = (
                    from entry in package.Data
                    let data = TransportDataSerialiser.Deserialise(entry.Data, entry.DataType)
                    select new SensorData(entry.DataType, entry.Model, data))
                    .ToList();

                // recompute all virtual sensors
                var vlist = _virtualSensors.Select(sensor => sensor.Compute(list)).ToList();
                list.AddRange(vlist); // add virtual sensor data

                // test
                Latest = new ReadOnlyCollection<SensorData>(list);
                DataIngested?.Invoke(Latest);

                // TODO: push to the repository
            }
        }
    }
}
