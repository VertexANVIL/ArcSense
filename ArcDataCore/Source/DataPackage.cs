using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MessagePack;

namespace ArcDataCore.Source
{
    [MessagePackObject]
    public class DataPackage : IDataPackage
    {
        public string Source { get; }
        public IReadOnlyList<(DateTime, object)> Data { get; }

        [IgnoreMember]
        private readonly IList<(DateTime, object)> _data = new List<(DateTime, object)>();

        public DataPackage(string source)
        {
            Source = source;
            Data = new ReadOnlyCollection<(DateTime, object)>(_data);
        }

        /// <summary>
        /// Appends data to the time-series list.
        /// </summary>
        /// <param name="dateTime">The date and time of this data point.</param>
        /// <param name="data">The data point.</param>
        public void Add(DateTime dateTime, object data) => _data.Add((dateTime, data));
    }
}
