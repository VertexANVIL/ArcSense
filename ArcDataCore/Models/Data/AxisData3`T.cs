using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace ArcDataCore.Models.Data
{
    [MessagePackObject]
    public struct AxisData3<T>
    {
        [Key(0)]
        public T X { get; set; }
        [Key(1)]
        public T Y { get; set; }
        [Key(2)]
        public T Z { get; set; }

        public AxisData3(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"{X:0.#},{Y:0.#},{Z:0.#}";
        }
    }
}
