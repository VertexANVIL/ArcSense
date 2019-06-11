namespace ArcSenseController.Sensors.Impl.Lsm303D
{
    /// <summary>
    /// Represents the full scales for magnetometer data in Gauss.
    /// </summary>
    internal enum Lsm303DMagnetometerScale
    {
        /// <summary>
        /// Magnetometer full scale of 2 Gauss.
        /// </summary>
        Scale2G = 0x0,

        /// <summary>
        /// Magnetometer full scale of 4 Gauss.
        /// </summary>
        Scale4G = 0x1,

        /// <summary>
        /// Magnetometer full scale of 8 Gauss.
        /// </summary>
        Scale8G = 0x2,

        /// <summary>
        /// Magnetometer full scale of 12 Gauss.
        /// </summary>
        Scale12G = 0x3
    }
}
