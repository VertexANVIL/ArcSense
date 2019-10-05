using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcSenseController.Services;

namespace ArcSenseController.Sensors.Impl.Bh1745
{
    internal class Bh1745Sensor : I2CSensor
    {
        private const byte BH1745_SLAVE_ADDRESS = 0x38;
        public Bh1745Sensor(I2CService i2c) : base(i2c) {}

        protected override Task InitialiseInternalAsync()
        {
            throw new NotImplementedException();
        }

        public override SensorModel Model { get; }
        public override (SensorDataType, object)[] Read() => new (SensorDataType, object) [] {
        };
    }
}
