namespace ArcSenseController.Sensors.Impl.As7262
{
    internal enum As7262ChannelType : sbyte
    {
        // Raw data
        Violet = 0x08,
        Blue = 0x0A,
        Green = 0x0C,
        Yellow = 0x0E,
        Orange = 0x10,
        Red = 0x12,

        // Calibrated data
        VioletCalibrated = 0x14,
        BlueCalibrated = 0x18,
        GreenCalibrated = 0x1C,
        YellowCalibrated = 0x20,
        OrangeCalibrated = 0x24,
        RedCalibrated = 0x28
    }
}
