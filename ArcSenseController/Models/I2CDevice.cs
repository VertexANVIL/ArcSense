using ArcSenseController.Services;

namespace ArcSenseController.Models
{
    public class I2CDevice
    {
        private readonly I2CService _service;
        private readonly byte _address;

        public I2CDevice(I2CService service, byte address) {
            _service = service;
            _address = address;
        }

        public void Read(byte register, byte[] buffer) {
            _service.Read(_address, register, buffer);
        }

        public void Read(byte register, byte buffer) {
            _service.Read(_address, register, new [] { buffer });
        }

        public void Write(byte register, byte[] buffer) {
            _service.Write(_address, register, buffer);
        }

        public void Write(byte register, byte buffer) {
            _service.Write(_address, register, new [] { buffer });
        }

        public void WriteRead(byte register, byte[] write, byte[] buffer) {
            _service.WriteRead(_address, register, write, buffer);
        }

        public void WriteRead(byte register, byte write, byte[] buffer) {
            WriteRead(register, new [] { write }, buffer);
        }
    }
}