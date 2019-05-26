namespace ArcSenseController.Models.Sensor.Impl.Lsm303D
{
    /// <summary>
    /// Operation modes for the Lsm303D magnetometer.
    /// </summary>
    internal enum Lsm303DMagnetometerMode
    {
        /// <summary>
        /// Continuous conversion mode.
        /// </summary>
        ContinuousConversion = 0x0,

        /// <summary>
        /// Single conversion mode.
        /// </summary>
        SingleConversion = 0x1,

        /// <summary>
        /// Powers down the magnetometer.
        /// </summary>
        PowerDown = 0x2,

        /// <summary>
        /// Do not use.
        /// </summary>
        PowerDown2 = 0x3
    }
}
