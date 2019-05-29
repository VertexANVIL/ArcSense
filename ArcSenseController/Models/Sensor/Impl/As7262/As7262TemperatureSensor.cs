using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcSenseController.Models.Sensor.Types;

namespace ArcSenseController.Models.Sensor.Impl.As7262
{
    internal class As7262TemperatureSensor : SubSensor<As7262Sensor>, ITemperatureSensor
    {
        internal As7262TemperatureSensor(As7262Sensor driver) : base(driver) { }

        public double Temperature => Driver.ReadTemperature();
    }
}
