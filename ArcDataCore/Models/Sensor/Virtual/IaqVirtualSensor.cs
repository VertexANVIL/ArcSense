using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArcDataCore.Analysis;

namespace ArcDataCore.Models.Sensor.Virtual
{
    /// <summary>
    /// Virtual sensor that calculates IAQ
    /// (Indoor Air Quality)
    /// </summary>
    public class IaqVirtualSensor : IVirtualSensor
    {
        private const int HUMID_OPT_FLOOR = 38;
        private const int HUMID_OPT_CEIL = 42;
        private const int HUMID_REF = 40;

        private const int GAS_OPT_FLOOR = 5000;
        private const int GAS_OPT_CEIL = 50000;
        private const int GAS_AVG_SIZE = 16;

        // Ratio of humidity to gas for calculating IAQ
        private const double HUMID_WEIGHT = 0.25;
        private const double GAS_WEIGHT = 0.75;

        private readonly Queue<double> _gasWindow = new Queue<double>();
        private double _gasAcc;

        public SensorData Compute(IReadOnlyCollection<SensorData> data)
        {
            double humidityScore;

            // Check whether humidity is optimal
            var humidity = (double) data.First(s => s.DataType == SensorDataType.RelativeHumidity).Data;
            if (humidity >= HUMID_OPT_FLOOR && humidity <= HUMID_OPT_CEIL)
            {
                humidityScore = HUMID_WEIGHT * 100;
            }
            else
            {
                humidityScore = humidity < HUMID_OPT_FLOOR 
                    ? HUMID_WEIGHT / HUMID_REF * humidity * 100 
                    : ((-HUMID_WEIGHT / (100 - HUMID_REF) * humidity) + 0.416666) * 100;
            }

            // Calculate gas values
            var gas = (double) data.First(s => s.DataType == SensorDataType.GasResistance).Data;
            gas = RecomputeGasAverage(gas);
            if (gas > GAS_OPT_CEIL) gas = GAS_OPT_CEIL;
            if (gas < GAS_OPT_FLOOR) gas = GAS_OPT_FLOOR;

            const double diff = (GAS_WEIGHT / (GAS_OPT_CEIL - GAS_OPT_FLOOR));
            var gasResScore = (diff * gas - (GAS_OPT_FLOOR * diff * 100));

            // Add up final IAQ score
            var score = humidityScore + gasResScore;
            return new SensorData(SensorDataType.VAirQualityIndex, SensorModel.Unknown, score);
        }

        /// <summary>
        /// Recomputes the gas average sliding window.
        /// </summary>
        private double RecomputeGasAverage(double value)
        {
            _gasAcc += value;
            _gasWindow.Enqueue(value);

            if (_gasWindow.Count > GAS_AVG_SIZE)
                _gasAcc -= _gasWindow.Dequeue();

            return _gasAcc / _gasWindow.Count;
        }
    }
}
