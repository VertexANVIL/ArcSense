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
using ArcSenseController.Models.Sensor.Impl;
using ArcSenseController.Models.Sensor.Impl.As7262;
using ArcSenseController.Models.Sensor.Impl.Bme680;
using ArcSenseController.Models.Sensor.Impl.Lsm303D;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace ArcSenseController
{
    public sealed class StartupTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //

            var deferral = taskInstance.GetDeferral();
            await TestSensors();
            deferral.Complete();
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
