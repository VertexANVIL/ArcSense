using ArcDataCore.Models.Data;

namespace ArcSenseController.Models.Sensor.Types
{
    internal interface ISpectralSensor
    {
        byte[] Spectrum { get; }
    }
}
