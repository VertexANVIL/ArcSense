using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcSenseController.Models.Sensor.Types
{
    /// <summary>
    /// Defines a sensor that has one or more sub sensors.
    /// </summary>
    internal interface ISplitSensor
    {
        IEnumerable<ISensor> SubSensors { get; }
    }
}
