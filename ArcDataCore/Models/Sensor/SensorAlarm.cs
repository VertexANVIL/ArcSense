using System;
using System.Collections.Generic;
using System.Text;

namespace ArcDataCore.Models.Sensor
{
    public enum SensorAlarm
    {
        Unknown, // No data or cannot compute
        Good,
        Moderate,
        Unhealthy,
        Hazardous,

    }
}
