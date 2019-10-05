using System.Threading.Tasks;

namespace ArcSenseController.Sensors.Impl.Bme680
{
    internal sealed class Bme680TemperatureSensor : HardwareSensor<Bme680Sensor>
    {
        /// <summary>
        /// Temperature oversampling.
        /// </summary>
        internal Bme680TemperatureOversamplings TemperatureOversampling = Bme680TemperatureOversamplings.x02;

        /// <summary>
        /// IIR filter.
        /// </summary>
        internal Bme680IirFilterCoefficients IirFilter = Bme680IirFilterCoefficients.FC_003;

        // Cal data
        private ushort _calT1;
        private short _calT2;
        private short _calT3;

        // Public cal data
        internal double FineTemperature;

        public Bme680TemperatureSensor(Bme680Sensor driver) : base(driver) { }

        protected override async Task InitialiseInternalAsync()
        {
            Calibrate();

            // Select IIR Filter for temperature sensor.
            Driver.WriteRegister((byte)Bme680Registers.CtrlIir, new byte[] { (byte)IirFilter });
            await Task.Delay(1);
        }

        private void Calibrate()
        {
            // Temperature calibration.
            _calT1 = (ushort)Driver.ReadRegister_TwoBytes_LSBFirst(Bme680Registers.CalT1Lsb);
            _calT2 = (short)Driver.ReadRegister_TwoBytes_LSBFirst(Bme680Registers.CalT2Lsb);
            _calT3 = Driver.ReadRegister_OneByte(Bme680Registers.CalT3);
        }

        /// <summary>
        /// Compensates the temperature.
        /// </summary>
        /// <param name="tempAdc">Analog temperature value.</param>
        /// <returns>Temperature in Celcius.</returns>
        internal double CompensateTemperature(int tempAdc)
        {
            var var1 = (tempAdc / 16384.0 - _calT1 / 1024.0) * _calT2;
            var var2 = (tempAdc / 131072.0 - _calT1 / 8192.0) * (tempAdc / 131072.0 - _calT1 / 8192.0) * (_calT3 * 16.0);
            FineTemperature = var1 + var2;
            var val = FineTemperature / 5120.0;

            return val;
        }

        /// <summary>
        /// Reads the temperature in celsius.
        /// </summary>
        /// <returns>Temperature in celsius.</returns>
        internal double ReadTempCelsius()
        {
            var adc = Driver.ReadRegister_OneByte(Bme680Registers.TempMsb) * 4096;
            adc += Driver.ReadRegister_OneByte(Bme680Registers.TempLsb) * 16;
            adc += Driver.ReadRegister_OneByte(Bme680Registers.TempXlsb) / 16;

            return CompensateTemperature(adc);
        }
    }
}
