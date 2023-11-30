using System.Text;
using AgrisysAirFeedingSystem.Models.DBModels;

namespace AgrisysAirFeedingSystem.Utils.LiveUpdate.Formatter;

public class EnumFormatter : Formatter
{
    private readonly string[] _enumOptions;
    public override string Id => "enum";
    public override string FormatInitialValue(SensorMeasurement measurement)
    {
        if ( 0 <= measurement.Value&& measurement.Value < _enumOptions.Length )
        {
            return _enumOptions[measurement.Value];
        }
        return "Undefined";
    }

    public override void SensorCheck(Sensor sensor)
    {
        if (sensor.max == null || sensor.min == null) return;
        
        if (sensor.max.Value >= _enumOptions.Length && sensor.min.Value < 0)
        {
            throw new ArgumentException("Sensor max and min is out of range");
        }
    }

    public override void AddAttributes(AttributeProvider measurement)
    {
        measurement.Add("data-sensor-options",string.Join(",", _enumOptions));
    }

    public EnumFormatter(Type enumType)
    {
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("TEnum must be an enumerated type");
        }
        
        var enumValues = Enum.GetValues(enumType);
        
        if (enumValues.Length < 1)
        {
            throw new ArgumentException("Enum must have at least one defined value");
        }

        var list = new string[enumValues.Length];

        for (int i = 0; i < enumValues.Length; i++)
        {
            list[i] = enumValues.GetValue(i)!.ToString() ?? throw new InvalidOperationException();
        }
        
        _enumOptions = list.ToArray();
    }
    
    public EnumFormatter(params string[] stringArray)
    {
        _enumOptions = stringArray;
    }
}