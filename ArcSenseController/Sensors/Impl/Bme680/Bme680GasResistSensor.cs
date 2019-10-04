using System;
using System.Threading.Tasks;
using ArcSenseController.Sensors.Types;

namespace ArcSenseController.Sensors.Impl.Bme680
{
    internal sealed class Bme680GasResistSensor : SubSensor<Bme680Sensor>, IGasResistanceSensor
    {
        /// <summary>
        /// Gas sensor heat duration in ms. Max value is 252.
        /// </summary>
        internal ushort HeatDuration = 150;

        /// <summary>
        /// Gas sensor heat temperature in C. Max value is 400.
        /// </summary>
        internal uint HeatTemperature = 320;

        // Data: Gas range constants for resistance calculation 
        private readonly double[] _constArray1 = { 1, 1, 1, 1, 1, 0.99, 1, 0.992, 1, 1, 0.998, 0.995, 1, 0.99, 1, 1 };
        private readonly double[] _constArray2 = { 8000000, 4000000, 2000000, 1000000, 499500.4995, 248262.1648, 125000, 63004.03226, 31281.28128, 15625, 7812.5, 3906.25, 1953.125, 976.5625, 488.28125, 244.140625 };

        // Calibration data
        private short _calGh1;
        private short _calGh2;
        private short _calGh3;

        private ushort _calResHeatRange;
        private short _calResHeatVal;
        private ushort _calRangeSwErr;

        private short _calAmbTemp = 25;
        public Bme680GasResistSensor(Bme680Sensor driver) : base(driver) { }

        protected override async Task InitialiseInternalAsync()
        {
            Calibrate();

            // Enable gas measurements.
            SetGasMeasurement(true);
            await Task.Delay(1);

            // Select index of heater set-point.
            SelectHeaterProfileSetPoint(Bme680HeaterProfileSetPoints.SP_0);
            await Task.Delay(1);

            // Define heater-on time.
            Driver.WriteRegister((byte)Bme680Registers.GasWait0, new byte[] { CalculateHeatDuration(HeatDuration) });
            await Task.Delay(1);

            // Set heater temperature.
            Driver.WriteRegister((byte)Bme680Registers.ResHeat0, new byte[] { CalculateHeaterResistance(HeatTemperature) });
            await Task.Delay(1);
        }

        private void Calibrate()
        {
            // Gas calibration.
            _calGh1 = Driver.ReadRegister_OneByte(Bme680Registers.CalGh1);
            _calGh2 = (short)Driver.ReadRegister_TwoBytes_LSBFirst(Bme680Registers.CalGh2Lsb);
            _calGh3 = Driver.ReadRegister_OneByte(Bme680Registers.CalGh3);

            // Heat calibration.
            _calResHeatRange = (ushort)((Driver.ReadRegister_OneByte(Bme680Registers.CalResHeatRange) & 0x30) / 16);
            _calResHeatVal = Driver.ReadRegister_OneByte(Bme680Registers.CalResHeatVal);
            _calRangeSwErr = (ushort)((Driver.ReadRegister_OneByte(Bme680Registers.CalRangeSwErr) & 0xF0) / 16);
        }

        /// <summary>
        /// Calculates the heater resistance value for target heater resistance (Res_Heat_X) registers.
        /// </summary>
        /// <param name="targetTemp">Target temperature.</param>
        /// <returns></returns>
        private byte CalculateHeaterResistance(uint targetTemp)
        {
            if (targetTemp > 400) // Maximum temperature
                targetTemp = 400;

            var var1 = (_calGh1 / 16.0) + 49.0;
            var var2 = ((_calGh2 / 32768.0) * 0.0005) + 0.00235;
            var var3 = _calGh3 / 1024.0;
            var var4 = var1 * (1.0 + (var2 * targetTemp));
            var var5 = var4 + (var3 * _calAmbTemp);
            var result = (byte)(3.4 * ((var5 * (4 / (4 + _calResHeatRange)) * (1 / (1 + (_calResHeatVal * 0.002)))) - 25));

            return result;
        }

        /// <summary>
        /// Calculates the heat duration value for gas sensor wait time (Gas_Wait_X) registers.
        /// </summary>
        /// <param name="dur">Wait duration in ms. Max value is 252.</param>
        /// <returns>Wait duration parameter in byte.</returns>
        private static byte CalculateHeatDuration(int dur)
        {
            ushort factor = 0, durval;

            if (dur >= 0xFC)
            {
                durval = 0xFF; // Max duration
            }
            else
            {
                while (dur > 0x3F)
                {
                    dur /= 4;
                    factor += 1;
                }

                durval = (ushort)(dur + (factor * 64));
            }

            return Convert.ToByte(durval);
        }

        /// <summary>
        /// Turns gas measurement on of off.
        /// </summary>
        /// <param name="state">Gas measurement mode. True for on, false for off.</param>
        internal void SetGasMeasurement(bool state)
        {
            var configValue = Driver.ReadRegister_OneByte(Bme680Registers.CtrlGas1);

            if (state)
                configValue |= 0b00010000;
            else
                configValue &= 0b11101111;

            Driver.WriteRegister((byte)Bme680Registers.CtrlGas1, new byte[] { configValue });
        }

        /// <summary>
        /// Selects heater set-points of the sensor that will be used in forced mode.
        /// </summary>
        /// <param name="heaterProfileSetPoint">Heater profile set-point.</param>
        private void SelectHeaterProfileSetPoint(Bme680HeaterProfileSetPoints heaterProfileSetPoint)
        {
            Driver.WriteRegister((byte)Bme680Registers.CtrlGas1, new byte[] { (byte)heaterProfileSetPoint });
        }

        /// <summary>
        /// Gets the gas measurement running status from the gas_measuring bit.
        /// </summary>
        /// <returns>True if gas measurement is running. False if not.</returns>
        internal bool GetGasMeasuringStatus()
        {
            var readValue = Driver.ReadRegister_OneByte(Bme680Registers.EasStatus0);
            return (readValue & 0b01000000) == 0b01000000;
        }

        /// <summary>
        /// Calculates the gas resistance value.
        /// </summary>
        /// <param name="gasResist">ADC resistance value.</param>
        /// <param name="gasRange">ADC range.</param>
        /// <returns>Gas resistance.</returns>
        private double CalculateGasResistance(int gasResist, ushort gasRange)
        {
            var var1 = (1340.0 + 5.0 * _calRangeSwErr) * _constArray1[gasRange];
            var result = var1 * _constArray2[gasRange] / (gasResist - 512.0 + var1);

            return result;
        }

        /// <summary>
        /// Reads the gas resistance.
        /// </summary>
        /// <returns>Gas resistance in Ohms.</returns>
        private double ReadGasResistance()
        {
            var tempAdc = Driver.ReadRegister_OneByte(Bme680Registers.TempMsb) * 4096;
            tempAdc += Driver.ReadRegister_OneByte(Bme680Registers.TempLsb) * 16;
            tempAdc += Driver.ReadRegister_OneByte(Bme680Registers.TempXlsb) / 16;
            var gasResAdc = Driver.ReadRegister_OneByte(Bme680Registers.GasRMsb) * 4;
            gasResAdc += Driver.ReadRegister_OneByte(Bme680Registers.GasRLsb) / 64;
            var gasRange = (ushort)(Driver.ReadRegister_OneByte(Bme680Registers.GasRLsb) & 0x0F);

            _calAmbTemp = Convert.ToInt16(Driver.TempSensor.CompensateTemperature(tempAdc));
            var val = CalculateGasResistance(gasResAdc, gasRange);

            return val;
        }

        public double Resistance => ReadGasResistance();
    }
}
