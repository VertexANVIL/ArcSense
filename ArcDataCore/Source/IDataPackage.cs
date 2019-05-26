using System;
using System.Collections.Generic;
using System.Text;

namespace ArcDataCore.Source
{
    /// <summary>
    /// Represents a package of time-series data with one or more attributes.
    /// </summary>
    public interface IDataPackage
    {
        /// <summary>
        /// The source from which the data in this package originates from.
        /// </summary>
        string Source { get; }

        /// <summary>
        /// Gets the time-series data contained within this package.
        /// </summary>
        IReadOnlyList<(DateTime, object)> Data { get; }
    }
}
