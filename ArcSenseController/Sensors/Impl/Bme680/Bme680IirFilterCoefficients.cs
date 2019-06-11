namespace ArcSenseController.Sensors.Impl.Bme680
{
    internal enum Bme680IirFilterCoefficients : byte
    {
        FC_000 = 0b00000000,
        FC_001 = 0b00000100,
        FC_003 = 0b00001000,
        FC_007 = 0b00001100,
        FC_015 = 0b00010000,
        FC_031 = 0b00010100,
        FC_063 = 0b00011000,
        FC_127 = 0b00011100
    }
}
