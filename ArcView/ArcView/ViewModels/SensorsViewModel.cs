using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArcDataCore.Analysis;
using ArcDataCore.Models.Sensor;
using ArcDataCore.Models.Sensor.Virtual;
using Xamarin.Forms;

using ArcView.Models;
using ArcView.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ArcView.ViewModels
{
    public class SensorsViewModel : BaseViewModel
    {
        public ObservableCollection<SensorTypeItem> Sensors { get; set; }
        private ISensorDataRepository _repository;

        private static readonly IReadOnlyCollection<IVirtualSensor> VirtualSensors
            = App.Services.GetServices<IVirtualSensor>().ToList();

        public SensorsViewModel()
        {
            Title = "Sensors";
            Sensors = new ObservableCollection<SensorTypeItem>
            {
                new SensorTypeItem { Type = SensorDataType.Accelerometer3D, Name = "Acceleration", Icon = "tachometer_alt.xml" },
                new SensorTypeItem { Type = SensorDataType.Magnetometer3D, Name = "Magnetism", Icon = "magnet.xml" },
                new SensorTypeItem { Type = SensorDataType.VAirQualityIndex, Name = "Air Quality", Icon = "chess_board.xml" },
                new SensorTypeItem { Type = SensorDataType.RelativeHumidity, Name = "Humidity", Icon = "tint.xml" },
                new SensorTypeItem { Type = SensorDataType.Pressure, Name = "Pressure", Icon = "compress_arrows_alt.xml" },
                new SensorTypeItem { Type = SensorDataType.Temperature, Name = "Temperature", Icon = "thermometer.xml" },
                //new SensorTypeItem { Type = SensorDataType.Spectral, Name = "Spectrometer", Icon = "palette.xml" },
                new SensorTypeItem { Type = SensorDataType.Colour, Name = "Colour", Icon = "palette.xml" },
                new SensorTypeItem { Type = SensorDataType.Radiation, Name = "Radiation", Icon = "radiation.xml" }
            };

            // really bad but fix later lol
            MessagingCenter.Subscribe<DataIngestProcessor>(this, "ingested", processor => UpdateSensors(processor.Latest));

            /**
            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });*/
        }

        private void UpdateSensors(IEnumerable<SensorData> data)
        {
            foreach (var point in data)
            {
                var item = Sensors.FirstOrDefault(s => s.Type == point.DataType);
                if (item == null) continue;

                item.Value = SensorDataFormatter.Format(point.Data, point.DataType);
            }
        }
    }
}