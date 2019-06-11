namespace ArcSenseController.Sensors.Types
{
    internal interface ISpectralSensor
    {
        byte[] Spectrum { get; }
    }
}
