using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.I2c;
using Windows.Devices.Enumeration;
using ArcDataCore.Source;
using ArcDataCore.Transport;
using ArcSenseController.Models.Data;
using ArcSenseController.Models.Data.Transport;
using ArcSenseController.Models.Sensor;
using ArcSenseController.Models.Sensor.Impl;
using ArcSenseController.Models.Sensor.Impl.As7262;
using ArcSenseController.Models.Sensor.Impl.Bme680;
using ArcSenseController.Models.Sensor.Impl.Gmc320;
using ArcSenseController.Models.Sensor.Impl.Lsm303D;
using ArcSenseController.Services;
using Microsoft.Extensions.DependencyInjection;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace ArcSenseController
{
    public sealed class StartupTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //

            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            var collection = new ServiceCollection();

            // Add transports
            collection.AddSingleton<IDataTransport, DebugTransport>();
            collection.AddSingleton<BluetoothTransport>();
            collection.AddSingleton<PairerService>();

            // Try to register all sensors
            await RegisterSensors(collection);

            collection.AddSingleton<IDataSourceService, DataSourceService>();
            collection.AddSingleton<SensorDataAdapter>();

            var provider = collection.BuildServiceProvider();

            var uploader = provider.GetRequiredService<IDataSourceService>();
            var adapter = provider.GetRequiredService<SensorDataAdapter>();
            var pairer = provider.GetRequiredService<PairerService>();

            // TODO: DEBUG: enable bluetooth
            var bluetooth = provider.GetRequiredService<BluetoothTransport>();
            await bluetooth.Initialise();

            uploader.Transport = bluetooth;

            // Start the application threads
            pairer.StartWatcher();
            adapter.Start();
            uploader.Start();

            // We don't want this to close, so delay infinitely
            await Task.Delay(-1);
        }

        private static async Task RegisterSensors(IServiceCollection services)
        {
            await services.RegisterSensorAsync<As7262Sensor>();
            await services.RegisterSensorAsync<Bme680Sensor>();
            //await services.RegisterSensorAsync<Gmc320Sensor>();
            await services.RegisterSensorAsync<Lsm303DSensor>();
        }

        private async Task TestSensors()
        {
            //var geiger = new Gmc320Sensor();

            var bme = new Bme680Sensor();
            await bme.InitialiseAsync();

            var gasresist = bme.GasSensor.Resistance;
            var pressure = bme.PressureSensor.Pressure;
            var humidity = bme.HumiditySensor.Humidity;
            var temperature = bme.TempSensor.Temperature;

            var lsm = new Lsm303DSensor();
            await lsm.InitialiseAsync();
            var accel = lsm.Accelerometer.Acceleration;
            var flux = lsm.Magnetometer.Flux;
            var temp = lsm.Magnetometer.Temperature;

            var asm = new As7262Sensor();
            await asm.InitialiseAsync();
        }
    }
}
