using System.Text;
using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Utils.LiveUpdate;
using AgrisysAirFeedingSystem.Utils.LiveUpdate.Formatter;
using AgrisysAirFeedingSystem.Utils.LiveUpdate.Handler;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace AgrisysAirFeedingSystem.Utils.TagHelper;

[HtmlTargetElement(Attributes = "hub-name")]  
public class LiveUpdateEventTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper  
{
    private readonly AgrisysDbContext _dbContext;

    public LiveUpdateEventTagHelper(AgrisysDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HtmlAttributeName("hub-name")]
    public string? Hub { get; set; }  
    
    [HtmlAttributeName("event-name")]
    public required int? Key { get; set; }  
    
    public override void Process(TagHelperContext context, TagHelperOutput output)  
    {  
        var attribute = new AttributeProvider();

        if (Key != null)
        {
            var group = _dbContext.Groups.FirstOrDefault(s => s.GroupId == Key);
        
            if (group == null)
            {
                throw new Exception("group not found");
            }
            
            attribute.Add("data-sensor-key",group.GroupId.ToString("X"));
        }
        else
        {
            attribute.Add("data-sensor-key","all");
        }
        
        var handler = new CustomHandler();
        handler.AddAttributes(attribute);
        output.Attributes.Add("data-sensor-handler",handler.id);

        if (Hub != null)
        {
            attribute.Add("data-sensor-hub",Hub);
        }
        
        attribute.WriteTo(output);
    }  
}  