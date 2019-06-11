namespace ArcSenseController.Sensors.Impl.Bme680
{
    internal enum Bme680TemperatureOversamplings : byte
    {
        Skipped = 0b00000000,
        x01 = 0b00100000,
        x02 = 0b01000000,
        x04 = 0b01100000,
        x08 = 0b10000000,
        x16 = 0b10100000
    }
}
