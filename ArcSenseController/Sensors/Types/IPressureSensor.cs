namespace ArcSenseController.Sensors.Types
{
    internal interface IPressureSensor
    {
        /// <summary>
        /// Returns the pressure in Pa (pascals).
        /// </summary>
        double Pressure { get; }

        /// <summary>
        /// Returns the altitude from the sea level in meters.
        /// </summary>
        /// <param name="seaLevelBar">Mean sea level pressure in bar. Will be used for altitude calculation from the pressure.</param>
        /// <returns>Altitude from the sea level in meters.</returns>
        //double GetAltitude(double seaLevelBar);
    }
}
