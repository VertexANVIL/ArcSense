using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ArcDataCore.Source;

namespace ArcDataCore.Transport
{
    /// <summary>
    /// Represents a method of uploading data to a source, aka, a transport.
    /// </summary>
    public interface IDataTransport
    {
        /// <summary>
        /// Commits a package to the transport's queue.
        /// </summary>
        /// <param name="package">The package to commit.</param>
        Task Commit(IDataPackage package);

        /// <summary>
        /// Flushes the transport, ensuring all packages are uploaded to the destination.
        /// If the transport times out or the connection is lost an error will be thrown.
        /// </summary>
        Task Flush();
    }
}
