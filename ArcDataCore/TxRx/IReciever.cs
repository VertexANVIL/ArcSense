using System.Threading;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Transport;

namespace ArcDataCore.TxRx
{
    public interface IReciever
    {
        Task<TransportDataPackage> PullAsync(CancellationToken? token);
    }
}
