using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using Xamarin.Forms;

using ArcView.Models;
using ArcView.Views;

namespace ArcView.ViewModels
{
    public class SensorsViewModel : BaseViewModel
    {
        public ObservableCollection<SensorTypeItem> Sensors { get; set; }

        public SensorsViewModel()
        {
            Title = "Sensors";
            Sensors = new ObservableCollection<SensorTypeItem>
            {
                new SensorTypeItem { Type = SensorDataType.Accelerometer3D, Name = "Acceleration", Icon = "tachometer_alt.xml" },
                new SensorTypeItem { Type = SensorDataType.Magnetometer3D, Name = "Magnetic Flux", Icon = "magnet.xml" },
                new SensorTypeItem { Type = SensorDataType.GasResistance, Name = "Gas Resistance", Icon = "chess_board.xml" },
                new SensorTypeItem { Type = SensorDataType.RelativeHumidity, Name = "Humidity", Icon = "tint.xml" },
                new SensorTypeItem { Type = SensorDataType.Pressure, Name = "Pressure", Icon = "compress_arrows_alt.xml" },
                new SensorTypeItem { Type = SensorDataType.Temperature, Name = "Temperature", Icon = "thermometer.xml" },
                //new SensorTypeItem { Type = SensorDataType.Spectral, Name = "Spectrometer", Icon = "palette.xml" },
                new SensorTypeItem { Type = SensorDataType.Colour, Name = "Colour", Icon = "palette.xml" },
                new SensorTypeItem { Type = SensorDataType.Radiation, Name = "Radiation", Icon = "radiation.xml" }
            };

            /**
            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });*/
        }
    }
}