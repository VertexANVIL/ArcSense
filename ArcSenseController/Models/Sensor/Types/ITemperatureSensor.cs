namespace ArcSenseController.Models.Sensor.Types
{
    /// <summary>
    /// Temperature sensor measuring celsius.
    /// </summary>
    internal interface ITemperatureSensor
    {
        /// <summary>
        /// Gets the current temperature in degrees celsius.
        /// </summary>
        double Temperature { get; }
    }
}
