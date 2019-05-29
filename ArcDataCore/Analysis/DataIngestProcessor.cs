using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using ArcDataCore.TxRx;

namespace ArcDataCore.Analysis
{
    public class DataIngestProcessor
    {
        public IReciever Reciever { get; set; }
        private Thread _thread;

        /// <summary>
        /// Starts the processor thread.
        /// </summary>
        public void Start()
        {
            _thread = new Thread(Process);
            _thread.Start();
        }

        private async void Process()
        {
            while (true)
            {
                // TODO: try/catch here, and log
                var package = await Reciever.PullAsync(null);
                Debug.WriteLine($"DIP: Ingesting package @ {package.TimeStamp} with {package.Data.Length} sensor datapoints");
            }
        }
    }
}
