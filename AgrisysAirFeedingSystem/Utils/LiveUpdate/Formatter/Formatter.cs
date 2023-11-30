using AgrisysAirFeedingSystem.Models.DBModels;

namespace AgrisysAirFeedingSystem.Utils.LiveUpdate.Formatter;

public abstract class Formatter
{
    public abstract string Id { get; }
    
    public abstract string FormatInitialValue(SensorMeasurement sensorMeasurement);
    
    public abstract void SensorCheck(Sensor sensor);
    
    public abstract void AddAttributes(AttributeProvider measurement);
}