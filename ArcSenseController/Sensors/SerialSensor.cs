using System.IO.Ports;

namespace ArcSenseController.Sensors
{
    internal abstract class SerialSensor : Sensor
    {
        /// <summary>
        /// The serial port name.
        /// </summary>
        public readonly string PortName;

        /// <summary>
        /// The baud rate of the port.
        /// </summary>
        public readonly int BaudRate;

        /// <summary>
        /// The serial port instance.
        /// </summary>
        protected SerialPort Port { get; private set; }

        public SerialSensor(string portName, int baudRate)
        {
            PortName = portName;
            BaudRate = baudRate;
            Open();

            Port.ErrorReceived += PortOnErrorReceived;
        }

        private void PortOnErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            
        }

        /// <summary>
        /// Opens the underlying serial port.
        /// </summary>
        protected void Open()
        {
            Port = new SerialPort(PortName, BaudRate, Parity.None, 8, StopBits.One);
            Port.Open();
        }
    }
}
