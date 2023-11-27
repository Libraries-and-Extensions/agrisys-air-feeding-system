using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgrisysAirFeedingSystem.Utils.LiveUpdate.Handler;

public abstract class BaseHandler
{
    public abstract string id { get; }
    
    public abstract void HandleInitialValue(string value,TagHelperOutput output);
    
    public abstract void AddAttributes(AttributeProvider measurement);
}