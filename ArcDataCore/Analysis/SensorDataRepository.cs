using System.Collections.Generic;
using ArcDataCore.Models.Sensor;

namespace ArcDataCore.Analysis
{
    public class SensorDataRepository : ISensorDataRepository
    {
        public IReadOnlyCollection<SensorData> Latest => _dip.Latest;

        private readonly DataIngestProcessor _dip;

        public SensorDataRepository(DataIngestProcessor dip)
        {
            _dip = dip;
        }
    }
}
