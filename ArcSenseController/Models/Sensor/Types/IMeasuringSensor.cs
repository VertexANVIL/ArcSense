using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcSenseController.Models.Sensor.Types
{
    /// <summary>
    /// Defines a sensor that a measure method must be called on
    /// to take measurements / refresh data. Measurements must complete before a read is allowed.
    /// </summary>
    internal interface IMeasuringSensor
    {
        void Measure();
    }
}
