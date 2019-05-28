using System;
using System.Threading.Tasks;
using ArcSenseController.Models.Sensor.Types;

namespace ArcSenseController.Models.Sensor.Impl.Bme680
{
    internal sealed class Bme680PressureSensor : SubSensor<Bme680Sensor>, IPressureSensor
    {
        /// <summary>
        /// Pressure oversampling.
        /// </summary>
        internal Bme680PressureOversamplings PressureOversampling = Bme680PressureOversamplings.x16;

        private ushort _calP1;
        private short _calP2;
        private short _calP3;
        private short _calP4;
        private short _calP5;
        private short _calP6;
        private short _calP7;
        private short _calP8;
        private short _calP9;
        private ushort _calP10;

        public Bme680PressureSensor(Bme680Sensor driver) : base(driver) { }

        public override Task InitialiseAsync()
        {
            Calibrate();
            return Task.CompletedTask;
        }

        private void Calibrate()
        {
            // Pressure calibration.
            _calP1 = (ushort)Driver.ReadRegister_TwoBytes_LSBFirst(Bme680Registers.CalP1Lsb);
            _calP2 = (short)Driver.ReadRegister_TwoBytes_LSBFirst(Bme680Registers.CalP2Lsb);
            _calP3 = Driver.ReadRegister_OneByte(Bme680Registers.CalP3);
            _calP4 = (short)Driver.ReadRegister_TwoBytes_LSBFirst(Bme680Registers.CalP4Lsb);
            _calP5 = (short)Driver.ReadRegister_TwoBytes_LSBFirst(Bme680Registers.CalP5Lsb);
            _calP6 = Driver.ReadRegister_OneByte(Bme680Registers.CalP6);
            _calP7 = Driver.ReadRegister_OneByte(Bme680Registers.CalP7);
            _calP8 = (short)Driver.ReadRegister_TwoBytes_LSBFirst(Bme680Registers.CalP8Lsb);
            _calP9 = (short)Driver.ReadRegister_TwoBytes_LSBFirst(Bme680Registers.CalP9Lsb);
            _calP10 = Driver.ReadRegister_OneByte(Bme680Registers.CalP10);
        }

        /// <summary>
        /// Compensates the pressure.
        /// </summary>
        /// <param name="presAdc">Analog pressure value.</param>
        /// <returns>Pressure.</returns>
        private double CompensatePressure(int presAdc)
        {
            var var1 = (Driver.TempSensor.FineTemperature / 2.0) - 64000.0;
            var var2 = var1 * var1 * (_calP6 / 131072.0);
            var2 += (var1 * _calP5 * 2.0);
            var2 = (var2 / 4.0) + (_calP4 * 65536.0);
            var1 = (((_calP3 * var1 * var1) / 16384.0) + (_calP2 * var1)) / 524288.0;
            var1 = (1.0 + (var1 / 32768.0)) * _calP1;
            double val = 1048576.0f - presAdc;

            if (var1 != 0)
            {
                val = ((val - (var2 / 4096.0)) * 6250.0) / var1;
                var1 = (_calP9 * val * val) / 2147483648.0;
                var2 = val * (_calP8 / 32768.0);
                var var3 = (val / 256.0) * (val / 256.0) * (val / 256.0) * (_calP10 / 131072.0);
                val += (var1 + var2 + var3 + (_calP7 * 128.0)) / 16.0;
            }
            else
            {
                val = 0;
            }

            return val;
        }

        /// <summary>
        /// Reads the pressure in Pa.
        /// </summary>
        /// <returns>Pressure in Pa.</returns>
        private double GetPressure()
        {
            var adc = Driver.ReadRegister_OneByte(Bme680Registers.PressMsb) * 4096;
            adc += Driver.ReadRegister_OneByte(Bme680Registers.PressLsb) * 16;
            adc += Driver.ReadRegister_OneByte(Bme680Registers.PressXlsb) / 16;

            return CompensatePressure(adc);
        }

        /// <summary>
        /// Reads the altitude from the sea level in meters.
        /// </summary>
        /// <param name="meanSeaLevelPressureInBar">Mean sea level pressure in bar. Will be used for altitude calculation from the pressure.</param>
        /// <returns>Altitude from the sea level in meters.</returns>
        public double ReadAltitude(double meanSeaLevelPressureInBar)
        {
            var phPa = GetPressure() / 100;
            return 44330.0 * (1.0 - Math.Pow((phPa / meanSeaLevelPressureInBar), 0.1903));
        }

        public double Pressure => GetPressure();
    }
}
