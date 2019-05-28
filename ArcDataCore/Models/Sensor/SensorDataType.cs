using System;
using System.Collections.Generic;
using System.Text;

namespace ArcDataCore.Models.Sensor
{
    /// <summary>
    /// Represents different sensor data types for server communication.
    /// </summary>
    public enum SensorDataType : ushort
    {
        /// <summary>
        /// Raw acceleration data as an array of three shorts.
        /// </summary>
        Accelerometer3D = 10,

        /// <summary>
        /// Raw flux data as an array of three shorts.
        /// </summary>
        Magnetometer3D = 11,

        /// <summary>
        /// Gas resistance as a double.
        /// </summary>
        GasResistance = 20,
        
        /// <summary>
        /// Relative humidity as a double.
        /// </summary>
        RelativeHumidity = 21,
        
        /// <summary>
        /// Pressure as a double.
        /// </summary>
        Pressure = 22,
        
        /// <summary>
        /// Temperature as a double.
        /// </summary>
        Temperature = 23,

        /// <summary>
        /// Spectral data as a MessagePack serialisation of <see cref="Data.Spectrum6"/>.
        /// </summary>
        Spectral = 25,

        /// <summary>
        /// Radiation as counts per minute.
        /// </summary>
        RadiationCpm = 30,
    }
}
