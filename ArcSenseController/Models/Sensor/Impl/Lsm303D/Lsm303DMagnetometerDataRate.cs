namespace ArcSenseController.Models.Sensor.Impl.Lsm303D
{
    /// <summary>
    /// The rate at which the Lsm303D magnetometer outputs data, in Hz.
    /// </summary>
    internal enum Lsm303DMagnetometerDataRate
    {
        /// <summary>
        /// Magnetometer output rate of 3.125Hz.
        /// </summary>
        Rate3Hz = 0x0,

        /// <summary>
        /// Magnetometer output rate of 6.25Hz.
        /// </summary>
        Rate6Hz = 0x1,

        /// <summary>
        /// Magnetometer output rate of 12.5Hz.
        /// </summary>
        Rate12Hz = 0x2,

        /// <summary>
        /// Magnetometer output rate of 25Hz.
        /// </summary>
        Rate25Hz = 0x3,

        /// <summary>
        /// Magnetometer output rate of 50Hz.
        /// </summary>
        Rate50Hz = 0x4,

        /// <summary>
        /// Magnetometer output rate of 100Hz.
        /// </summary>
        Rate100Hz = 0x5
    }
}
