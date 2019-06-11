using System;
using System.Collections.Generic;
using System.Text;
using ArcDataCore.Models.Sensor.Virtual;
using Microsoft.Extensions.DependencyInjection;

namespace ArcDataCore.Helpers
{
    public static class ServiceProviderExtensions
    {
        private static readonly IReadOnlyList<Type> Sensors = new List<Type>
        {
            typeof(IaqVirtualSensor)
        };

        public static void AddVirtualSensors(this IServiceCollection collection)
        {
            foreach (var type in Sensors) collection.AddSingleton(typeof(IVirtualSensor), type);
        }
    }
}
