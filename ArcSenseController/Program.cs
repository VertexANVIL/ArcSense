using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using ArcDataCore.Transport;
using ArcDataCore.TxRx;
using ArcSenseController.Models.Data;
// using ArcSenseController.Models.Data.Transport;
using ArcSenseController.Sensors;
using ArcSenseController.Sensors.Impl.As7262;
using ArcSenseController.Sensors.Impl.Bme680;
using ArcSenseController.Sensors.Impl.Lsm303D;
using ArcSenseController.Sensors.Impl.Gmc320;
using ArcSenseController.Sensors.Types;
using ArcSenseController.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace ArcSenseController
{
    public sealed class Program
    {
        static void Main(string[] args) {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            Debug.WriteLine("Service initialisation...");
            var collection = new ServiceCollection();

            // Add I2C
            //var i2c = new I2CService();
            //i2c.InitBus();

            //collection.AddSingleton(i2c);

            // Add transports
            collection.AddSingleton<DebugTransmitter>();

            // Try to register all sensors
            RegisterSensors(collection);

            collection.AddSingleton<ITransmissionService, TransmissionService>();
            collection.AddSingleton<SensorDataAdapter>();

            var provider = collection.BuildServiceProvider();

            var uploader = provider.GetRequiredService<ITransmissionService>();
            var adapter = provider.GetRequiredService<SensorDataAdapter>();

            // TODO: DEBUG: enable bluetooth
            //var bluetooth = provider.GetRequiredService<BluetoothService>();
            //await bluetooth.Initialise();

            uploader.Transport = provider.GetRequiredService<DebugTransmitter>();

            // Initialise sensors
            Debug.WriteLine("Sensor initialisation...");
            await InitialiseSensors(provider);

            // Start the application threads
            adapter.Start();
            uploader.Start();

            Debug.WriteLine("Ready");

            // We don't want this to close, so delay infinitely
            await Task.Delay(-1);
        }

        private static void RegisterSensors(IServiceCollection services)
        {
            //services.AddSingleton<As7262Sensor>();
            services.AddSingleton<Bme680Sensor>();
            //services.AddSingleton<ISensor>(new Gmc320Sensor("/dev/ttyUSB0"));
            services.AddSingleton<Lsm303DSensor>();
        }

        private static async Task InitialiseSensors(IServiceProvider provider) {
            var sensors = provider.GetServices<ISensor>();
            foreach (var sensor in sensors) {
                await sensor.InitialiseAsync();
            }
        }
    }
}
