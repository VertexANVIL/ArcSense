namespace ArcSenseController.Sensors.Impl.Lsm303D
{
    internal enum Lsm303DRegisters
    {
        // Temperature registers
        TempOutL = 0x5,
        TempOutH = 0x6,

        // Start of magnetic data registers
        OutXlm = 0x8,

        /// <summary>
        /// Returns device ID. Should be 49 (hex).
        /// </summary>
        WhoAmI = 0xF,

        /// <summary>
        /// Interrupt control register.
        /// </summary>
        /// <remarks>
        /// XMIEN: Enable interrupt recognition on X-axis for magnetic data
        /// YMIEN: Enable interrupt recognition on Y-axis for magnetic data
        /// ZMIEN: Enable interrupt recognition on Y-axis for magnetic data
        /// PP_OD: Interrupt in configuration
        /// MIEA: Interrupt polarity
        /// MIEL: Latch interrupt request
        /// </remarks>
        IntCtrlM = 0x12,
        IntSrcM = 0x13,
        IntThsLm = 0x14,
        IntThsHm = 0x15,

        OffsetXlm = 0x16,
        OffsetXhm = 0x17,
        OffsetYlm = 0x18,
        OffsetYhm = 0x19,
        OffsetZlm = 0x1A,
        OffsetZhm = 0x1B,

        ReferenceX = 0x1C,
        ReferenceY = 0x1D,
        ReferenceZ = 0x1E,

        /// <summary>
        /// Sets the system control register.
        /// </summary>
        Ctrl0 = 0x1F,

        Ctrl1 = 0x20,
        Ctrl2 = 0x21,
        Ctrl3 = 0x22,
        Ctrl4 = 0x23,
        Ctrl5 = 0x24,
        Ctrl6 = 0x25,
        Ctrl7 = 0x26,

        StatusA = 0x27,

        // Start of accelerometer data registers
        OutXla = 0x28
    }
}
