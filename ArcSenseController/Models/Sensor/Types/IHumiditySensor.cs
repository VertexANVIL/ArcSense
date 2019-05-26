namespace ArcSenseController.Models.Sensor.Types
{
    internal interface IHumiditySensor
    {
        /// <summary>
        /// Returns the RH (Relative Humidity) expressed as a percentage of the amount needed for saturation at the same temperature.
        /// </summary>
        double Humidity { get; }
    }
}
