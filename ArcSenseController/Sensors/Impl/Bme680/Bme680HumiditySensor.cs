using System.Threading.Tasks;
using ArcSenseController.Sensors.Types;

namespace ArcSenseController.Sensors.Impl.Bme680
{
    internal sealed class Bme680HumiditySensor : SubSensor<Bme680Sensor>, IHumiditySensor
    {
        /// <summary>
        /// Humidity oversampling.
        /// </summary>
        internal Bme680HumidityOversamplings HumidityOversampling = Bme680HumidityOversamplings.x02;

        private ushort _calH1;
        private ushort _calH2;
        private short _calH3;
        private short _calH4;
        private short _calH5;
        private ushort _calH6;
        private short _calH7;

        public Bme680HumiditySensor(Bme680Sensor driver) : base(driver) { }

        public override async Task InitialiseAsync()
        {
            Calibrate();

            // Select humidity oversampling.
            Driver.WriteRegister(new[] { (byte)Bme680Registers.CtrlHumidity, (byte)HumidityOversampling });
            await Task.Delay(1);
        }

        private void Calibrate()
        {
            // Humidity calibration.
            _calH1 = (ushort)(Driver.ReadRegister_OneByte(Bme680Registers.CalH1Msb) << 4 | (Driver.ReadRegister_OneByte(Bme680Registers.CalH1Lsb) & 0x0F));
            _calH2 = (ushort)(Driver.ReadRegister_OneByte(Bme680Registers.CalH2Msb) << 4 | (Driver.ReadRegister_OneByte(Bme680Registers.CalH2Lsb) >> 4));
            _calH3 = Driver.ReadRegister_OneByte(Bme680Registers.CalH3);
            _calH4 = Driver.ReadRegister_OneByte(Bme680Registers.CalH4);
            _calH5 = Driver.ReadRegister_OneByte(Bme680Registers.CalH5);
            _calH6 = Driver.ReadRegister_OneByte(Bme680Registers.CalH6);
            _calH7 = Driver.ReadRegister_OneByte(Bme680Registers.CalH7);
        }

        /// <summary>
        /// Compensates the humidity.
        /// </summary>
        /// <param name="humAdc">Analog humidity value.</param>
        /// <returns>Relative humidity.</returns>
        private double CompensateHumidity(int humAdc)
        {
            var tempComp = Driver.TempSensor.FineTemperature / 5120.0;
            var var1 = humAdc - ((_calH1 * 16.0) + ((_calH3 / 2.0) * tempComp));
            var var2 = var1 * (((_calH2 / 262144.0) * (1.0 + ((_calH4 / 16384.0) * tempComp) + ((_calH5 / 1048576.0) * tempComp * tempComp))));
            var var3 = _calH6 / 16384.0;
            var var4 = _calH7 / 2097152.0;
            var val = var2 + ((var3 + (var4 * tempComp)) * var2 * var2);

            if (val > 100.0)
                val = 100.0;
            else if (val < 0.0)
                val = 0.0;

            return val;
        }

        /// <summary>
        /// Reads the relative humidity.
        /// </summary>
        /// <returns>Relative humidity.</returns>
        private double GetHumidity()
        {
            var adc = Driver.ReadRegister_OneByte(Bme680Registers.HumMsb) * 256;
            adc += Driver.ReadRegister_OneByte(Bme680Registers.HumLsb);

            return CompensateHumidity(adc);
        }

        public double Humidity => GetHumidity();
    }
}
