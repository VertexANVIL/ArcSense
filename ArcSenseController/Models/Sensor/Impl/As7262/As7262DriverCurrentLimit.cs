using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcSenseController.Models.Sensor.Impl.As7262
{
    internal enum As7262DriverCurrentLimit : sbyte
    {
        Limit12Ma5 = 0b00,
        Limit25Ma = 0b01,
        Limit50Ma = 0b10,
        Limit100Ma = 0b11
    }
}
