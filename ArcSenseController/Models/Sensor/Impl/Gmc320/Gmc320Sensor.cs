using System;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArcDataCore.Models.Sensor;
using ArcSenseController.Models.Sensor.Types;

namespace ArcSenseController.Models.Sensor.Impl.Gmc320
{
    /// <summary>
    /// Implementation of the GQ Geiger Counter Communication protocol
    /// GQ-RFC1201 Ver 1.40
    /// </summary>
    internal class Gmc320Sensor : SerialSensor, IGeigerSensor, ITemperatureSensor
    {
        public string FirmwareModel { get; private set; }
        public string FirmwareVersion { get; private set; }

        public Gmc320Sensor(string portName, int baudRate = 115200) : base(portName, baudRate) { }

        public override Task InitialiseAsync()
        {
            Open();

            var version = GetVersion();
            FirmwareModel = version.Substring(0, 7);
            FirmwareVersion = version.Substring(7);

            return Task.CompletedTask;
        }

        public override SensorModel Model => SensorModel.Gmc320;

        public string GetVersion()
        {
            SendStr("GETVER");
            return ReadString(14);
        }

        public int GetCpm()
        {
            SendStr("GETCPM");
            return (ReadByte() << 8) | ReadByte();
        }

        public int GetBatteryVoltage()
        {
            SendStr("GETVOLT");
            return ReadByte();
        }

        public byte[] GetHistoryData(int blockSize = 1024, int readTimeout = 500)
        {
            var data = new byte[65536];
            var numberOfBlocks = data.Length / blockSize;

            var offset = 0;
            for (var blockNumber = 0; blockNumber < numberOfBlocks; blockNumber++)
            {
                SendStr("SPIR", (offset >> 16) & 0xFF, (offset >> 8) & 0xFF, offset & 0xFF, (blockSize >> 8) & 0xFF, blockSize & 0xFF);
                Thread.Sleep(readTimeout);

                var bytesRead = Port.Read(data, offset, blockSize);
                if (bytesRead != blockSize) throw new Exception($"Received {bytesRead:N0} bytes, expected {blockSize:N0} bytes");

                // TODO: is it a bug in firmware? it returns 1 byte more than requested
                var extraBytes = Port.BytesToRead;
                for (var i = 0; i < extraBytes; i++) ReadByte();

                offset += blockSize;
            }

            return data;
        }

        public byte[] GetConfigurationData()
        {
            SendStr("GETCFG");
            return ReadBytes(256);
        }

        public void WriteConfigurationData(byte[] data)
        {
            throw new NotImplementedException();
        }

        public void EraseConfigurationData()
        {
            SendStr("ECFG");
            ReadAaByte();
        }

        public void ReloadConfigurationData()
        {
            SendStr("CFGUPDATE");
            ReadAaByte();
        }

        public void SendKey(Gmc320Keys key)
        {
            SendStr("KEY", (int)key);
            Thread.Sleep(500);
        }

        public string GetSerial()
        {
            SendStr("GETSERIAL");
            return ReadHexString(7);
        }

        public void PowerOff()
        {
            SendStr("POWEROFF");
        }

        public void PowerOn()
        {
            SendStr("POWERON");
        }

        public void Reboot()
        {
            SendStr("REBOOT");
        }

        public void SendKeys(Gmc320Keys key, params Gmc320Keys[] keys)
        {
            SendKey(key);
            foreach (var t in keys) SendKey(t);
        }

        public void SetDateTime(DateTime dateTime)
        {
            SendStr("SETDATETIME",
                 dateTime.Year % 100, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, 0xAA);

            ReadAaByte();
        }

        public DateTime GetDateTime()
        {
            SendStr("GETDATETIME");

            var deviceDateTime = new DateTime(2000 + ReadByte(), ReadByte(), ReadByte(), ReadByte(), ReadByte(), ReadByte(), DateTimeKind.Local);
            ReadAaByte();

            return deviceDateTime;
        }

        private void SendStr(string command, params int[] parameters)
        {
#if DEBUG
            Trace.Write("<" + command);
            if (parameters.Length > 0)
            {
                Trace.Write("[");
                foreach (var t in parameters) System.Diagnostics.Trace.Write($"{t:X2},");
                Trace.Write("]");
            }

            Trace.WriteLine(">>");
#endif

            Port.Write("<" + command);

            if (parameters.Length > 0)
            {
                var bytes = Array.ConvertAll(parameters, b => (byte)b);
                Port.Write(bytes, 0, bytes.Length);
            }

            Port.WriteLine(">>");
        }

        /// <summary>
        /// Gets the current temperature.
        /// </summary>
        public double GetCelsius()
        {
            var result = ReadBytes(3);
            ReadAaByte();

            var temp = double.Parse($"{result[0]}.{result[1]}");
            if (result[2] != 0) temp *= -1;
            return temp;
        }

        /// <summary>
        /// Gets the current gyroscope position.
        /// </summary>
        public Vector<int> GetGyroVector()
        {
            var x = (ReadByte() << 8) | ReadByte();
            var y = (ReadByte() << 8) | ReadByte();
            var z = (ReadByte() << 8) | ReadByte();
            ReadAaByte();

            return new Vector<int>(new []{x, y, z});
        }

        private byte ReadByte()
        {
            var b = (byte)Port.ReadByte();
            return b;
        }

        private byte[] ReadBytes(int length)
        {
            var bytes = new byte[length];
            for (var i = 0; i < length; i++) bytes[i] = ReadByte();

            return bytes;
        }

        private string ReadString(int length)
        {
            var stringBuilder = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                stringBuilder.Append((char)ReadByte());
            }

            return stringBuilder.ToString();
        }

        private string ReadHexString(int length)
        {
            var stringBuilder = new StringBuilder(length * 2);
            for (var i = 0; i < length; i++)
            {
                stringBuilder.AppendFormat("{0:X2}", ReadByte());
            }

            return stringBuilder.ToString();
        }

        private void ReadAaByte()
        {
            var b = ReadByte();
            if (b != 0xAA) throw new Exception($"Received byte 0x{b:X2}, expected byte 0xAA");
        }

        public double Temperature { get; }
    }
}
