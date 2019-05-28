using System;
using Microsoft.Extensions.Configuration;

namespace ArcSenseController.Models.Config
{
    internal sealed class ControllerSection : ConfigurationSection
    {
        public ControllerSection(ConfigurationRoot root, string path) : base(root, path) { }

        public TransportType Transport
        {
            get => (TransportType)Enum.Parse(typeof(TransportType), this["transport"]);
            set => this["transport"] = value.ToString();
        }
    }
}
