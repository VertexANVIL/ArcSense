using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcSenseController.Models.Sensor.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ArcSenseController.Services
{
    internal static class SensorServiceExtensions
    {
        public static async Task RegisterSensorAsync<T>(this IServiceCollection collection) where T: ISensor, new()
        {
            var sensor = new T();
            await sensor.InitialiseAsync();

            collection.AddSingleton<ISensor>(sensor);

            // Add split sensors if necessary
            if (sensor is ISplitSensor split)
                foreach (var subSensor in split.SubSensors)
                    collection.AddSingleton(subSensor);
        }
    }
}
