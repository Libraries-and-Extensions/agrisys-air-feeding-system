using System.Text;
using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Utils.LiveUpdate;
using AgrisysAirFeedingSystem.Utils.LiveUpdate.Formatter;
using AgrisysAirFeedingSystem.Utils.LiveUpdate.Handler;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace AgrisysAirFeedingSystem.Utils.Taghelper;

[HtmlTargetElement(Attributes = "sensor-name")]  
public class LiveUpdateTagHelper : TagHelper  
{
    private readonly AgrisysDbContext _dbContext;

    public LiveUpdateTagHelper(AgrisysDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HtmlAttributeName("sensor-format")]
    public Formatter? Formatter { get; set; }  
    [HtmlAttributeName("sensor-handler")]
    public BaseHandler? Handler { get; set; } 
    
    [HtmlAttributeName("hub-name")]
    public string? Hub { get; set; }  
    
    [HtmlAttributeName("sensor-name")]
    public required string Key { get; set; }  
    
    public override void Process(TagHelperContext context, TagHelperOutput output)  
    {  
        if (Key == null)
        {
            throw new Exception("Key is required");
        }
        
        var sensor = _dbContext.Sensors.FirstOrDefault(s => s.Name == Key);
        
        if (sensor == null)
        {
            throw new Exception("Sensor not found");
        }
        
        var lastMeasurement = _dbContext.Measurements
            .Where(m=>m.SensorId == sensor.SensorId)
            .OrderByDescending(c => c.TimeStamp)
            .FirstOrDefault(m => m.SensorId == sensor.SensorId);
        
        var attribute = new AttributeProvider();
        
        attribute.Add("data-sensor-key",sensor.SensorId.ToString("X"));
        
        var value = "no_value";
        
        if (Formatter != null)
        {
            Formatter.SensorCheck(sensor);
            Formatter.AddAttributes(attribute);
            output.Attributes.Add("data-sensor-format",Formatter.Id);

            if (lastMeasurement != null) value = Formatter.FormatInitialValue(lastMeasurement);
        }
        
        if (Handler != null)
        {
            Handler.AddAttributes(attribute);
            output.Attributes.Add("data-sensor-handler",Handler.id);
            
            Handler.HandleInitialValue(value,output,lastMeasurement);
        }else
        {
            output.Content.SetContent(value);
        }

        if (Hub != null)
        {
            attribute.Add("data-sensor-hub",Hub);
        }
        
        attribute.WriteTo(output);
    }  
}  