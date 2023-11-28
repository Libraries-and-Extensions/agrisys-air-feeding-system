using System.Text;
using AgrisysAirFeedingSystem.Models.DBModels;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgrisysAirFeedingSystem.Utils.LiveUpdate.Handler;

public class CssClassHandler : BaseHandler
{
    private readonly string? _clazz;
    public override string id => "cssClass";
    public override void HandleInitialValue(string value, TagHelperOutput output,SensorMeasurement? measurement)
    {
        StringBuilder builder = new();
        
        if (output.Attributes.TryGetAttribute("class", out var attribute))
        {
            builder.Append(attribute);
        }
        
        if (!string.IsNullOrEmpty(_clazz))
        {
            builder.Append(" " + _clazz.Replace("{value}",value));
        }
        else
        {
            builder.Append(" " + value);
        }
        
        output.Attributes.SetAttribute("class",builder.ToString().TrimStart());
        output.Attributes.SetAttribute("data-old-css-class",builder.ToString().TrimStart());
    }

    public override void AddAttributes(AttributeProvider attribute)
    {
        attribute.Add("data-css-class", _clazz);
    }

    public CssClassHandler(string? clazz = null)
    {
        if (clazz != null && !clazz.Contains("{value}"))
        {
            throw new Exception("cssClass must contain {value}");
        }
        
        _clazz = clazz;
    }
}