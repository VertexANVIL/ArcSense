using ArcDataCore.Models.Data;

namespace ArcSenseController.Models.Sensor.Types
{
    internal interface ISpectralSensor
    {
        Spectrum6 Spectrum { get; }
    }
}
