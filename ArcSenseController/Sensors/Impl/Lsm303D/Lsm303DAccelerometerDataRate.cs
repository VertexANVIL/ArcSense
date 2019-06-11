namespace ArcSenseController.Sensors.Impl.Lsm303D
{
    /// <summary>
    /// The rate at which the Lsm303D accelerometer outputs data, in Hz.
    /// </summary>
    internal enum Lsm303DAccelerometerDataRate
    {
        /// <summary>
        /// Represents the sensor turned off.
        /// </summary>
        Zero = 0x0,

        /// <summary>
        /// An output rate of 3.125Hz.
        /// </summary>
        Rate3Hz = 0x1,

        /// <summary>
        /// An output rate of 6.25Hz.
        /// </summary>
        Rate6Hz = 0x2,

        /// <summary>
        /// An output rate of 12.5Hz.
        /// </summary>
        Rate12Hz = 0x3,

        /// <summary>
        /// An output rate of 25Hz.
        /// </summary>
        Rate25Hz = 0x4,

        /// <summary>
        /// An output rate of 50Hz.
        /// </summary>
        Rate50Hz = 0x5,

        /// <summary>
        /// An output rate of 100Hz.
        /// </summary>
        Rate100Hz = 0x6,

        /// <summary>
        /// An output rate of 200Hz.
        /// </summary>
        Rate200Hz = 0x7,

        /// <summary>
        /// An output rate of 400Hz.
        /// </summary>
        Rate400Hz = 0x8,

        /// <summary>
        /// An output rate of 800Hz.
        /// </summary>
        Rate800Hz = 0x9,

        /// <summary>
        /// An output rate of 1600Hz.
        /// </summary>
        Rate1600Hz = 0xa
    }
}
