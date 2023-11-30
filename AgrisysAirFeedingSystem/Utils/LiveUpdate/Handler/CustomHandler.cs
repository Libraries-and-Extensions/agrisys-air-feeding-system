using AgrisysAirFeedingSystem.Models.DBModels;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace AgrisysAirFeedingSystem.Utils.LiveUpdate.Handler;

public class CustomHandler : BaseHandler
{
    public override string id => "custom";
    public override void HandleInitialValue(string value, TagHelperOutput output,SensorMeasurement? measurement)
    {
        if (measurement == null) return;
        
        var data = new
        {
            measurement.TimeStamp,
            measurement.Value,
            key=measurement.SensorId.ToString("X"),
            formatted = value
        }.ToJToken();
        data["formatted"] = value;
        
        output.Attributes.SetAttribute("data-custom-initial", data.ToString(Formatting.None));
    }

    public override void AddAttributes(AttributeProvider measurement)
    {
        //nothing to add
    }
}