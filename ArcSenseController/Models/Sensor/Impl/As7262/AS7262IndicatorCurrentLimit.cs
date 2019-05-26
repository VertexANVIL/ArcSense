using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcSenseController.Models.Sensor.Impl.As7262
{
    internal enum As7262IndicatorCurrentLimit : sbyte
    {
        Limit1Ma = 0b00,
        Limit2Ma = 0b01,
        Limit4Ma = 0b10,
        Limit8Ma = 0b11
    }
}
