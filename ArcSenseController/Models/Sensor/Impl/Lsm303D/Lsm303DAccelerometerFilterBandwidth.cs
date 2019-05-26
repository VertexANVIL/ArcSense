namespace ArcSenseController.Models.Sensor.Impl.Lsm303D
{
    /// <summary>
    /// Represents the AA (anti-alias) filter bandwith for magnetometer data from the LSM303D.
    /// </summary>
    internal enum Lsm303DAccelerometerFilterBandwidth
    {
        /// <summary>
        /// Filter bandwidth of 773 Hz.
        /// </summary>
        Bandwidth773Hz = 0x0,

        /// <summary>
        /// Filter bandwidth of 194 Hz.
        /// </summary>
        Bandwidth194Hz = 0x1,

        /// <summary>
        /// Filter bandwidth of 362 Hz.
        /// </summary>
        Bandwidth362Hz = 0x2,

        /// <summary>
        /// Filter bandwidth of 50 Hz.
        /// </summary>
        Bandwidth50Hz = 0x3
    }
}
