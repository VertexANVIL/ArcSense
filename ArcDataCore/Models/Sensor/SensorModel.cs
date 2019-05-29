namespace ArcDataCore.Models.Sensor
{
    public enum SensorModel : ushort
    {
        Unknown = 0,

        // Spectrometer
        [SensorType(SensorPriority.Highest, SensorDataType.Spectral)]
        As7262 = 100,

        // Air quality sensor
        [SensorType(SensorPriority.Highest)]
        Bme680 = 200,

        // Colour sensor
        [SensorType(SensorPriority.Highest, SensorDataType.Colour)]
        Bh1745 = 300,

        // 6 DoF eCompass
        [SensorType(SensorPriority.Highest, SensorDataType.Accelerometer3D)]
        [SensorType(SensorPriority.Highest, SensorDataType.Magnetometer3D)]
        Lsm303D = 400,

        // Geiger counter
        [SensorType(SensorPriority.Highest, SensorDataType.Radiation)]
        Gmc320 = 500,
    }
}
