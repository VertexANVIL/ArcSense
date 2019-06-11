namespace ArcSenseController.Sensors.Impl.Lsm303D
{
    /// <summary>
    /// Represents the full scales for accelerometer data in G-force.
    /// </summary>
    internal enum Lsm303DAccelerometerScale
    {
        /// <summary>
        /// Accelerometer full scale of 2 Gs.
        /// </summary>
        Scale2G = 0x0,

        /// <summary>
        /// Accelerometer full scale of 4 Gs.
        /// </summary>
        Scale4G = 0x1,

        /// <summary>
        /// Accelerometer full scale of 6 Gs.
        /// </summary>
        Scale6G = 0x2,

        /// <summary>
        /// Accelerometer full scale of 8 Gs.
        /// </summary>
        Scale8G = 0x3,

        /// <summary>
        /// Accelerometer full scale of 16 Gs.
        /// </summary>
        Scale16G = 0x4
    }
}
