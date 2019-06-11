using ArcSenseController.Sensors.Types;

namespace ArcSenseController.Sensors.Impl.As7262
{
    internal class As7262TemperatureSensor : SubSensor<As7262Sensor>, ITemperatureSensor
    {
        internal As7262TemperatureSensor(As7262Sensor driver) : base(driver) { }

        public double Temperature => Driver.ReadTemperature();
    }
}
