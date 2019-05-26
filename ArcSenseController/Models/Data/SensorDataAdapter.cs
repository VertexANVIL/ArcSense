using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArcDataCore;
using ArcSenseController.Models.Sensor.Types;

namespace ArcSenseController.Models.Data
{
    internal class SensorDataAdapter
    {
        /// <summary>
        /// Interval in milliseconds in which polling should take place.
        /// </summary>
        private const int PollInterval = 1000;

        /// <summary>
        /// Represents all sensors registered on this adapter instance.
        /// </summary>
        private readonly ICollection<ISensor> _sensors;

        /// <summary>
        /// Represents the data source that data should be sent to.
        /// </summary>
        private readonly IDataSourceService _source;

        private readonly Timer _timer;

        public SensorDataAdapter(IDataSourceService service, ICollection<ISensor> sensors)
        {
            _sensors = sensors;
            _timer = new Timer(PollSensors, null, 0, PollInterval);
        }

        private void PollSensors(object state)
        {
            foreach (var sensor in _sensors) Poll(sensor);
        }

        private void Poll(ISensor sensor)
        {
            var model = (int) sensor.Model;

            
        }
    }
}
