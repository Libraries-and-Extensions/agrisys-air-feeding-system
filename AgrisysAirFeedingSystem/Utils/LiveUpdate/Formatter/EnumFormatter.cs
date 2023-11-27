using System.Text;
using AgrisysAirFeedingSystem.Models.DBModels;

namespace AgrisysAirFeedingSystem.Utils.LiveUpdate.Formatter;

public class EnumFormatter : Formatter
{
    private readonly string[] _enumOptions;
    public override string Id => "enum";
    public override string FormatInitialValue(SensorMeasurement measurement)
    {
        if (measurement.Value >= _enumOptions.Length && measurement.Value < 0)
        {
            return _enumOptions[measurement.Value];
        }
        //TODO: what should be return if value is out of range
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
        
        StringBuilder sb = new(enumValues.GetValue(0)!.ToString());

        for (int i = 1; i < enumValues.Length; i++)
        {
            list[i] = (string)enumValues.GetValue(i)!;
        }
        
        _enumOptions = list.ToArray();
    }
    
    public EnumFormatter(params string[] stringArray)
    {
        _enumOptions = stringArray;
    }
}