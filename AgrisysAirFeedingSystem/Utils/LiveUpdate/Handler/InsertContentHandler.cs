using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgrisysAirFeedingSystem.Utils.LiveUpdate.Handler;

public class ContentHandler : BaseHandler
{
    private readonly string? _format;
    private readonly string? _prefix;
    private readonly string? _suffix;
    public override string id => "content";
    public override void HandleInitialValue(string value, TagHelperOutput output)
    {
        StringBuilder builder = new();
        
        if (_format != null)
        {
            builder.Append(_format.Replace("{value}",value));
        }
        else if (_prefix == null || _suffix == null)
        {
            if (_prefix != null) builder.Append(_prefix);
            builder.Append(value);
            if (_suffix != null) builder.Append(_suffix);
        }
        else
        {
            builder.Append(value);
        }
        
        output.Content.SetContent(builder.ToString());
    }

    public override void AddAttributes(AttributeProvider measurement)
    {
        if (_format != null) measurement.Add("data-content-format", _format);
        if (_prefix != null) measurement.Add("data-content-prefix", _prefix);
        if (_suffix != null) measurement.Add("data-content-suffix", _suffix);
    }

    public ContentHandler(string? prefix=null,string? suffix = null)
    {
        _prefix = prefix;
        _suffix = suffix;
    }
    
    public ContentHandler(string format)
    {
        _format = format;
    }
    
}