using System.Text;
using AgrisysAirFeedingSystem.Models.DBModels;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgrisysAirFeedingSystem.Utils.LiveUpdate.Handler;

public class CssStyleHandler : BaseHandler
{
    private readonly string _style;
    private readonly string _unit;
    public override string id => "cssStyle";
    public override void HandleInitialValue(string value, TagHelperOutput output,SensorMeasurement? measurement)
    {
        StringBuilder builder = new();
        if (output.Attributes.TryGetAttribute("style", out var attribute))
        {
            builder.Append(attribute.Value);
        }
        
        builder.Append($"{_style}:{value}{_unit}");
        
        output.Attributes.SetAttribute("style",builder.ToString());
    }

    public override void AddAttributes(AttributeProvider attribute)
    {
        attribute.Add("data-style-handler-property", _style);
        attribute.Add("data-style-handler-unit", _unit);
    }

    public CssStyleHandler(string style,string unit)
    {
        _style = style;
        _unit = unit;
    }
}