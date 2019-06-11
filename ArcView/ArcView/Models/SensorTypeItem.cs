using System;
using System.Collections.Generic;
using System.Text;
using ArcDataCore.Models.Sensor;
using Xamarin.Forms;

namespace ArcView.Models
{
    public class SensorTypeItem : BindableObject
    {
        public SensorDataType Type { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        /// <summary>
        /// The current value of the sensor.
        /// </summary>
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        private string _value = "NaN";
    }
}
