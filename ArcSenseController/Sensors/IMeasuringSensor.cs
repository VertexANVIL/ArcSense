namespace ArcSenseController.Sensors
{
    /// <summary>
    /// Defines a sensor that a measure method must be called on
    /// to take measurements / refresh data. Measurements must complete before a read is allowed.
    /// </summary>
    internal interface IMeasuringSensor
    {
        void Measure();
    }
}
