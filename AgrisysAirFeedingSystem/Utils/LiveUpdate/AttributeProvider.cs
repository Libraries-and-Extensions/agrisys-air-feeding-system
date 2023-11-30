using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgrisysAirFeedingSystem.Utils.LiveUpdate;

public class AttributeProvider
{
    readonly Dictionary<string,string> _attributes = new();
    
    public void Add(string key, string value)
    {
        _attributes.Add(key,value);
    }
    
    public void Add(string key, int value)
    {
        Add(key,value.ToString());
    }
    
    public void Remove(string key)
    {
        _attributes.Remove(key);
    }
    
    public bool TryGet(string key, out string? value)
    {
        return _attributes.TryGetValue(key, out value);
    }
    
    public string? Get(string key)
    {
        return _attributes.TryGetValue(key, out string? value) ? value : null;
    }
    
    public bool Has(string key)
    {
        return _attributes.ContainsKey(key);
    }
    
    public void WriteTo(TagHelperOutput output)
    {
        foreach (var attribute in _attributes)
        {
            output.Attributes.Add(attribute.Key,attribute.Value);
        }
    }
}