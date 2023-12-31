﻿using System.Globalization;
using System.Text;
using AgrisysAirFeedingSystem.Models.DBModels;

namespace AgrisysAirFeedingSystem.Utils.LiveUpdate.Formatter;

public class NumberFormatter : Formatter
{
    private readonly int _scale;
    private int? _digit;
    public override string Id => "number";
    
    public NumberFormatter(int scale, int? digit = null)
    {
        _scale = scale;
        _digit = digit;
    }
    
    public override string FormatInitialValue(SensorMeasurement measurement)
    {
        var value = (double)measurement.Value/_scale;
        if (_digit.HasValue)
        {
            value = Math.Round(value, _digit.Value);
        }
        
        return value.ToString(CultureInfo.CurrentCulture);
    }

    public override void SensorCheck(Sensor sensor)
    {
        //only adjust if digit is not specified
        if (_digit.HasValue) return;
        
        var max = sensor.max;
        var min = sensor.min;
        
        //only adjust if both max and min is not null
        if (max == null || min == null) return;
        
        var diff = max.Value - min.Value;

        //adjust for scale
        diff = diff / _scale;

        _digit = diff switch
        {
            < 1 => 2,
            < 10 => 1,
            _ => 0
        };
    }

    public override void AddAttributes( AttributeProvider attributeProvider)
    {
        attributeProvider.Add("data-sensor-scale-factor",_scale);
        
        if (_digit.HasValue) attributeProvider.Add("data-sensor-scale-digit",_digit.Value);
    }
}