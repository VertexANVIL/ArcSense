using System;
using System.Collections.Generic;
using System.Text;

namespace ArcDataCore.Models.Sensor
{
    public static class SensorDataFormatter
    {
        /// <summary>
        /// Provides a custom formatter for displaying primitive sensor data in a UI.
        /// </summary>
        /// <param name="data">The sensor data.</param>
        /// <param name="value">The sensor data type.</param>
        public static string Format(object data, SensorDataType value)
        {
            // We don't need all the cases intentionally.
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch(value)
            {
                case SensorDataType.GasResistance:
                    return $"{data:0.##} Ω";
                case SensorDataType.RelativeHumidity:
                    return $"{data:0.###}% RH";
                case SensorDataType.Pressure:
                    return $"{data:0.#} Pa";
                case SensorDataType.Temperature:
                    return $"{data:0.####}°C";
                case SensorDataType.Radiation:
                    return $"{data} CPM";
                case SensorDataType.VAirQualityIndex:
                    return $"{data:0.##} IAQ";
                default:
                    return data.ToString();
            }
        }
    }
}
