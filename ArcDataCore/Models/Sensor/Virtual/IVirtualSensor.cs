using System;
using System.Collections.Generic;
using System.Text;

namespace ArcDataCore.Models.Sensor.Virtual
{
    public interface IVirtualSensor
    {
        /// <summary>
        /// Recomputes the value of the virtual sensor.
        /// </summary>
        SensorData Compute(IReadOnlyCollection<SensorData> data);
    }
}
