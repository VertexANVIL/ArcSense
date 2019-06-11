using System.Collections.Generic;

namespace ArcSenseController.Sensors.Types
{
    /// <summary>
    /// Defines a sensor that has one or more sub sensors.
    /// </summary>
    internal interface ISplitSensor
    {
        IEnumerable<ISensor> SubSensors { get; }
    }
}
