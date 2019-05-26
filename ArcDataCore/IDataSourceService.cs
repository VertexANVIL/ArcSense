using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Source;
using ArcDataCore.Transport;

namespace ArcDataCore
{
    public interface IDataSourceService
    {
        void RegisterTransport(IDataTransport transport);

        Task Enqueue(SensorData data);
    }
}
