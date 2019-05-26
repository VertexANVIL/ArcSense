using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcSenseController.Models.Sensor.Types;

namespace ArcSenseController.Models.Sensor
{
    internal abstract class Sensor : ISensor
    {
        public abstract Task InitialiseAsync();

        public abstract SensorModel Model { get; }
    }
}
