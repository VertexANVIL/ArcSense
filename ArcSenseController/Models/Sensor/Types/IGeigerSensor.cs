namespace ArcSenseController.Models.Sensor.Types
{
    /// <summary>
    /// Defines a sensor able to detect amounts of ionising radiation.
    /// </summary>
    internal interface IGeigerSensor
    {
        /// <summary>
        /// The counts per minute of ionising radiation detected by the sensor.
        /// </summary>
        int GetCpm();
    }
}
