using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgrisysAirFeedingSystem.Utils.LiveUpdate.Handler;

public class cssClassHandler : BaseHandler
{
    private readonly string _cssClass;
    public override string id => "cssClass";
    public override void HandleInitialValue(string value, TagHelperOutput output)
    {
        StringBuilder builder = new();
        
        if (output.Attributes.TryGetAttribute("class", out var attribute))
        {
            builder.Append(attribute);
        }
        
        if (!string.IsNullOrEmpty(_cssClass))
        {
            builder.Append(" " + _cssClass.Replace("{value}",value));
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
        attribute.Add("data-css-class", _cssClass);
    }

    public cssClassHandler(string cssClass)
    {
        _cssClass = cssClass;
    }
}