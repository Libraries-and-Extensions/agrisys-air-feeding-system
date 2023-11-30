using System.Globalization;
using AgrisysAirFeedingSystem.Models.DBModels;

namespace AgrisysAirFeedingSystem.Utils.LiveUpdate.Formatter;

public class NumberMapFormatter : Formatter
{
    private readonly int _outputMax;
    private int? _inputMax;
    private readonly int? _outputMin;
    private int? _inputMin;
    private readonly int _digit;
    public override string Id => "numberMap";
    public override string FormatInitialValue(SensorMeasurement measurement)
    {
        var value = MathExtentions.Map(
            _inputMin ?? 0,
            _inputMax ?? throw new NullReferenceException("inputMax is null"), 
            _outputMin ?? 0, 
            _outputMax, 
            measurement.Value);
        
        value = Math.Round(value, _digit);
            
        return value.ToString(CultureInfo.CurrentCulture);
    }

    public override void SensorCheck(Sensor sensor)
    {
        _inputMax ??= sensor.max;
        _inputMin ??= sensor.min;
    }

    public override void AddAttributes(AttributeProvider measurement)
    {
        if (_inputMax == null) throw new NullReferenceException("inputMax is null");
        
        measurement.Add("data-sensor-map-in-max", _inputMax.ToString()!);
        measurement.Add("data-sensor-map-out-max",_outputMax.ToString());
        
        if (_inputMin.HasValue)  measurement.Add("data-sensor-map-in-min",_inputMin.ToString()!);
        if (_outputMin.HasValue)  measurement.Add("data-sensor-map-out-min",_outputMin.ToString()!);
        if (_digit != 0)  measurement.Add("data-sensor-scale-digit",_digit.ToString());
    }

    public NumberMapFormatter(int outputMax,int? inputMax=null, int? outputMin=null,int? inputMin=null, int digit=0)
    {
        _outputMax = outputMax;
        _inputMax = inputMax;
        _outputMin = outputMin;
        _inputMin = inputMin;
        _digit = digit;
    }
}