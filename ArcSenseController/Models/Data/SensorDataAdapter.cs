using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArcDataCore;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Transport;
using ArcDataCore.TxRx;
using ArcSenseController.Sensors;
using ArcSenseController.Services;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;

namespace ArcSenseController.Models.Data
{
    internal class SensorDataAdapter
    {
        /// <summary>
        /// Interval in milliseconds in which polling should take place.
        /// </summary>
        private const int POLL_INTERVAL = 1000;

        /// <summary>
        /// Represents all sensors registered on this adapter instance.
        /// </summary>
        private readonly IList<HardwareSensor> _sensors;

        /// <summary>
        /// Represents the data source that data should be sent to.
        /// </summary>
        private readonly ITransmissionService _service;

        private Timer _timer;
        private readonly object _timerLock = new object();

        public SensorDataAdapter(IServiceProvider services)
        {
            _service = services.GetService<ITransmissionService>();
            _sensors = services.GetServices<HardwareSensor>().ToList();
        }

        public void Start()
        {
            _timer = new Timer(PollSensors, null, 0, POLL_INTERVAL);
        }

        private void PollSensors(object state)
        {
            // STOP the timer until we are finished
            if (!Monitor.TryEnter(_timerLock)) return;

            try
            {
                var now = DateTime.UtcNow;
                var list = new List<TransportData>();

                var sw = new Stopwatch();
                sw.Start();
                foreach (var sensor in _sensors) Poll(sensor, list);
                sw.Stop();

                _service.Commit(new TransportDataPackage(now, list.ToArray()));
                Debug.WriteLine($"Full sensor poll completed in {sw.ElapsedMilliseconds} ms.");
            }
            catch (Exception e)
            {
                // write some exception text
                Debug.WriteLine($"Failed to poll sensors: \"{e.Message}\"");
            }
            finally
            {
                Monitor.Exit(_timerLock);
            }
        }

        /// <summary>
        /// Polls a sensor.
        /// </summary>
        /// <param name="sensor">The sensor to use.</param>
        /// <param name="dest">The destination collection to add the data to.</param>
        private void Poll(HardwareSensor sensor, ICollection<TransportData> dest)
        {
            if (!sensor.Initialised) return;

            var sw = new Stopwatch();
            sw.Start();

            // Run the sensor measurements (if required)
            if (sensor is IMeasuringSensor measure) measure.Measure();
            var data = sensor.Read();

            sw.Stop();

            //Debug.WriteLine($"Read sensor {sensor.Model} in {sw.ElapsedMilliseconds} ms at {now}");

            // Commit all
            foreach (var (dataType, obj) in data) {
                Debug.WriteLine($"TYPE: {dataType}");
                Debug.WriteLine($"DATA: {Convert.ToString(obj)}");
                dest.Add(new TransportData(dataType, sensor.Model, MessagePackSerializer.Serialize(obj)));
            }

            // Enforce a mandatory delay of 10ms between polls, to prevent I2C lockups
            Task.Delay(10).Wait();
        }
    }
}
