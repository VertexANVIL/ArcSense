namespace ArcSenseController.Sensors.Types
{
    /// <summary>
    /// Represents a sensor that is able
    /// to read gas resistance as ohms.
    /// </summary>
    internal interface IGasResistanceSensor
    {
        /// <summary>
        /// Returns the gas resistance in Ω (ohms).
        /// </summary>
        /// <returns>The resistance.</returns>
        double Resistance { get; }
    }
}
