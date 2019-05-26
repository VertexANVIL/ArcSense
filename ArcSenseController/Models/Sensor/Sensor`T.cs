using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcSenseController.Models.Sensor.Types;

namespace ArcSenseController.Models.Sensor
{
    /// <summary>
    /// Represents a sensor that is a subset of a master driver.
    /// </summary>
    /// <typeparam name="T">The type of the driver.</typeparam>
    internal abstract class Sensor<T> : Sensor where T: ISensor
    {
        protected readonly T Driver;
        internal Sensor(T driver)
        {
            Driver = driver;
        }

        public override SensorModel Model => Driver.Model;
    }
}
