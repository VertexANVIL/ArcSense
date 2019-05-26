using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArcDataCore.Source;
using ArcDataCore.Transport;

namespace ArcSenseController.Models.Data.Transport
{
    internal class BluetoothTransport : IDataTransport
    {
        

        public Task Commit(IDataPackage package)
        {
            

            return Task.CompletedTask;
        }

        public Task Flush()
        {
            throw new System.NotImplementedException();
        }
    }
}
