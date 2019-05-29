using System.Threading;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;

namespace ArcDataCore.TxRx
{
    public interface IReciever
    {
        Task<SensorDataPackage> PullAsync(CancellationToken? token);
    }
}
