namespace ArcSenseController.Sensors.Impl.As7262
{
    internal enum As7262ConversionType : sbyte
    {
        Default = Continuous0,

        /// <summary>
        /// Data will be available in registers V, B, G & Y
        /// </summary>
        Continuous0 = 0b00,

        /// <summary>
        /// Data will be available in registers G, Y, O & R
        /// </summary>
        Continuous1 = 0b01,

        /// <summary>
        /// Data will be available in registers V, B, G, Y, O & R
        /// </summary>
        Continuous2 = 0b10,

        /// <summary>
        /// Data will be available in registers V, B, G, Y, O & R in One Shot mode.
        /// After measurement is completed the DATA_RDY bit is set to 1.
        /// </summary>
        OneShot = 0b11
    }
}
