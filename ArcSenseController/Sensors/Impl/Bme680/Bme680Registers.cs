namespace ArcSenseController.Sensors.Impl.Bme680
{
    internal enum Bme680Registers : byte
    {
        // Readout
        EasStatus0 = 0x1D,
        PressMsb = 0x1F,
        PressLsb = 0x20,
        PressXlsb = 0x21,
        TempMsb = 0x22,
        TempLsb = 0x23,
        TempXlsb = 0x24,
        HumMsb = 0x25,
        HumLsb = 0x26,
        GasRMsb = 0x2A,
        GasRLsb = 0x2B,

        /// <summary>
        /// Selects the sensor power-down mode.
        /// </summary>
        Mode = 0x74,

        /// <summary>
        /// Selects the SPI memory page.
        /// </summary>
        Status = 0x73,

        /// <summary>
        /// Writing 0xB6 to this register initiates a soft-reset procedure.
        /// </summary>
        Reset = 0xE0,

        /// <summary>
        /// Returns the chip ID.
        /// </summary>
        Id = 0xD0,

        /// <summary>
        /// Controls the IIR filter. [config]
        /// </summary>
        CtrlIir = 0x75,

        /// <summary>
        /// Sets temperature and pressure oversampling data. [ctrl_meas]
        /// </summary>
        CtrlMeasurement = 0x74,

        /// <summary>
        /// Sets humidity oversampling data. [ctrl_hum]
        /// </summary>
        CtrlHumidity = 0x72,

        CtrlGas0 = 0x70,
        CtrlGas1 = 0x71,
        
        GasWait0 = 0x64,
        GasWait1 = 0x65,
        GasWait2 = 0x66,
        GasWait3 = 0x67,
        GasWait4 = 0x68,
        GasWait5 = 0x69,
        GasWait6 = 0x6A,
        GasWait7 = 0x6B,
        GasWait8 = 0x6C,
        GasWait9 = 0x6D,

        ResHeat0 = 0x5A,
        ResHeat1 = 0x5B,
        ResHeat2 = 0x5C,
        ResHeat3 = 0x5D,
        ResHeat4 = 0x5E,
        ResHeat5 = 0x5F,
        ResHeat6 = 0x60,
        ResHeat7 = 0x61,
        ResHeat8 = 0x62,
        ResHeat9 = 0x63,

        IdacHeat0 = 0x50,
        IdacHeat1 = 0x51,
        IdacHeat2 = 0x52,
        IdacHeat3 = 0x53,
        IdacHeat4 = 0x54,
        IdacHeat5 = 0x55,
        IdacHeat6 = 0x56,
        IdacHeat7 = 0x57,
        IdacHeat8 = 0x58,
        IdacHeat9 = 0x59,

        // Calibration
        CalResHeatVal = 0x00,
        CalResHeatRange = 0x02,
        CalRangeSwErr = 0x04,

        CalT2Lsb = 0x8A,
        CalT2Msb = 0x8B,
        CalT3 = 0x8C,
        CalP1Lsb = 0x8E,
        CalP1Msb = 0x8F,
        CalP2Lsb = 0x90,
        CalP2Msb = 0x91,
        CalP3 = 0x92,
        CalP4Lsb = 0x94,
        CalP4Msb = 0x95,
        CalP5Lsb = 0x96,
        CalP5Msb = 0x97,
        CalP7 = 0x98,
        CalP6 = 0x99,
        CalP8Lsb = 0x9C,
        CalP8Msb = 0x9D,
        CalP9Lsb = 0x9E,
        CalP9Msb = 0x9F,
        CalP10 = 0xA0,
        CalH2Msb = 0xE1,
        CalH2Lsb = 0xE2,
        CalH1Lsb = 0xE2,
        CalH1Msb = 0xE3,
        CalH3 = 0xE4,
        CalH4 = 0xE5,
        CalH5 = 0xE6,
        CalH6 = 0xE7,
        CalH7 = 0xE8,
        CalT1Lsb = 0xE9,
        CalT1Msb = 0xEA,
        CalGh2Lsb = 0xEB,
        CalGh2Msb = 0xEC,
        CalGh1 = 0xED,
        CalGh3 = 0xEE,
    }
}
