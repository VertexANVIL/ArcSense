namespace ArcSenseController.Sensors.Impl.As7262
{
    internal enum As7262ChannelGain : sbyte
    {
        Default = Gain1X,

        Gain1X = 0b00,
        Gain3X7 = 0b01,
        Gain16X = 0b10,
        Gain64X = 0b11
    }
}
