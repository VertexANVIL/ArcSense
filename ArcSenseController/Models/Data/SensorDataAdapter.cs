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
using ArcSenseController.Sensors.Types;
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
        private readonly IList<ISensor> _sensors;

        /// <summary>
        /// Represents the data source that data should be sent to.
        /// </summary>
        private readonly ITransmissionService _service;

        private Timer _timer;
        private readonly object _timerLock = new object();

        public SensorDataAdapter(IServiceProvider services)
        {
            _service = services.GetService<ITransmissionService>();
            _sensors = services.GetServices<ISensor>().ToList();
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
                // TOOD: write some exception text
                throw;
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
        private void Poll(ISensor sensor, ICollection<TransportData> dest)
        {
            var list = new List<(object, SensorDataType)>();

            var sw = new Stopwatch();
            sw.Start();

            // Run the sensor measurements (if required)
            if (sensor is IMeasuringSensor measure) measure.Measure();

            // Read the data from the sensor (if required)
            switch (sensor)
            {
                case IAccelerometerSensor accelerometer:
                    list.Add((accelerometer.Acceleration, SensorDataType.Accelerometer3D));
                    break;
                case IMagnetometerSensor magnetometer:
                    list.Add((magnetometer.Flux, SensorDataType.Magnetometer3D));
                    break;
                case IGasResistanceSensor gasResist:
                    list.Add((gasResist.Resistance, SensorDataType.GasResistance));
                    break;
                case IHumiditySensor humidity:
                    list.Add((humidity.Humidity, SensorDataType.RelativeHumidity));
                    break;
                case IPressureSensor pressure:
                    list.Add((pressure.Pressure, SensorDataType.Pressure));
                    break;
                case ITemperatureSensor temperature:
                    list.Add((temperature.Temperature, SensorDataType.Temperature));
                    break;
                case ISpectralSensor spectral:
                    list.Add((spectral.Spectrum, SensorDataType.Spectral));
                    break;
                case IGeigerSensor geiger:
                    list.Add((geiger.Cpm, SensorDataType.Radiation));
                    break;
            }

            sw.Stop();

            //Debug.WriteLine($"Read sensor {sensor.Model} in {sw.ElapsedMilliseconds} ms at {now}");

            // Commit all
            foreach (var (obj, dataType) in list)
                dest.Add(new TransportData(dataType, sensor.Model, MessagePackSerializer.Serialize(obj)));

            // Enforce a mandatory delay of 10ms between polls, to prevent I2C lockups
            Task.Delay(10).Wait();
        }
    }
}
