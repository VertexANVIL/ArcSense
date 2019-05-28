using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace ArcDataCore.Models.Data
{
    [MessagePackObject]
    public struct Spectrum6
    {
        [Key(0)]
        public float Violet;
        [Key(1)]
        public float Blue;
        [Key(2)]
        public float Green;
        [Key(3)]
        public float Yellow;
        [Key(4)]
        public float Orange;
        [Key(5)]
        public float Red;
    }
}
