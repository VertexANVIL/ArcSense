namespace ArcSenseController.Models.Sensor.Types
{
    internal interface ISpectralSensor
    {
        double Violet { get; }
        double Blue { get; }
        double Green { get; }
        double Yellow { get; }
        double Orange { get; }
        double Red { get; }
    }
}
