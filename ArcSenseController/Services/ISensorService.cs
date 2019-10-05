using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcSenseController.Sensors;

namespace ArcSenseController.Services
{
    internal interface ISensorService
    {
        IEnumerable<HardwareSensor> Sensors { get; }
    }
}
