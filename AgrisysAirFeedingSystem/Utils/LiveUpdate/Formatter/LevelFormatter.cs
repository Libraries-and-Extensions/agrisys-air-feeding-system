using System.Text;
using AgrisysAirFeedingSystem.Models.DBModels;

namespace AgrisysAirFeedingSystem.Utils.LiveUpdate.Formatter;

public class LevelFormatter : Formatter
{
    private readonly Dictionary<int, string> _levels;
    public override string Id => "level";
    public override string FormatInitialValue(SensorMeasurement measurement)
    {
        string value =  "Unknown";

        foreach (var level in _levels)
        {
            if (level.Key > measurement.Value)
            {
               break;
            }

            value = level.Value;
        }

        return value;
    }

    public override void SensorCheck(Sensor sensor)
    {
        /*if (sensor.max.Value >= _levels.ElementAt(_levels.Count - 1).Key || sensor.min.Value < _levels.ElementAt(0).Key)
        {
            throw new ArgumentException("Sensor max and min is out of range");
        }*/
    }

    public override void AddAttributes(AttributeProvider measurement)
    {
        measurement.Add("data-sensor-levels", string.Join(",", _levels.Select(l => $"{l.Key}:{l.Value}")));
    }
    
    public LevelFormatter(Dictionary<int,string> levels)
    {
        _levels = levels;
    }
}