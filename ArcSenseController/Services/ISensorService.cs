using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcSenseController.Models.Sensor.Types;

namespace ArcSenseController.Services
{
    internal interface ISensorService
    {
        IEnumerable<ISensor> Sensors { get; }
    }
}
