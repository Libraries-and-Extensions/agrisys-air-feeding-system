using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgrisysAirFeedingSystem.Utils.TagHelper;


[HtmlTargetElement(Attributes = "long-press")]  
public class LongPressTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
{
    [HtmlAttributeName("long-press")]
    [AspMvcAction, LocalizationRequired(false)]
    public string? Action {get; set;}
    
    [HtmlAttributeName("long-press-controller")]
    [AspMvcController, LocalizationRequired(false)]
    public string? Controller {get; set;}
    
    [HtmlAttributeName("long-press-timout")]
    public int? Timeout {get; set;}
    
    [HtmlAttributeName("long-press-prevent-click")]
    public bool? PreventClick {get; set;}
    
    [HtmlAttributeName(DictionaryAttributePrefix = "long-press-url-")]
    public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();


    [ViewContext]
    [HtmlAttributeNotBound]
    public required ViewContext ViewContext { get; set; }
    
    private readonly IUrlHelperFactory _urlFactory;

    public LongPressTagHelper(IUrlHelperFactory urlHelperFactory)
    {
        _urlFactory = urlHelperFactory;
    }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var urlHelper = _urlFactory.GetUrlHelper(ViewContext);
        
        output.Attributes.SetAttribute("data-long-press", urlHelper.Action(Action, Controller, PageUrlValues));
        if (Timeout.HasValue) output.Attributes.SetAttribute("data-long-press-timeout", Timeout.Value);
        if (PreventClick.HasValue) output.Attributes.SetAttribute("data-long-press-prevent-click", PreventClick.Value);
    }
}