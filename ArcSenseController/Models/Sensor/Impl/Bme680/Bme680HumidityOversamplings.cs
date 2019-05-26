namespace ArcSenseController.Models.Sensor.Impl.Bme680
{
    internal enum Bme680HumidityOversamplings : byte
    {
        Skipped = 0b00000000,
        x01 = 0b00000001,
        x02 = 0b00000010,
        x04 = 0b00000011,
        x08 = 0b00000100,
        x16 = 0b00000101
    }
}
