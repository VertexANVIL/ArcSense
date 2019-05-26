namespace ArcSenseController.Models.Sensor.Impl.Bme680
{
    internal enum Bme680PressureOversamplings : byte
    {
        Skipped = 0b00000000,
        x01 = 0b00000100,
        x02 = 0b00001000,
        x04 = 0b00001100,
        x08 = 0b00010000,
        x16 = 0b00010100
    }
}
