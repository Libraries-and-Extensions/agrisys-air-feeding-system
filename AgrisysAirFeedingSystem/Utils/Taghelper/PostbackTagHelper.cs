using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;

namespace AgrisysAirFeedingSystem.Utils.TagHelper;

// data-postback="@Url.Action("ChangeRole","UserApi")" data-postback-parameter="role" data-postback-request-data="id:@user.Identity.Id"
[HtmlTargetElement(Attributes = "postback")]
public class PostbackTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
{
    [HtmlAttributeName("postback")]
    [AspMvcAction, LocalizationRequired(false)]
    public required string? Action {get; set;}
    
    [HtmlAttributeName("postback-parameter-name")]
    public required string ParameterName {get; set;}
    
    [HtmlAttributeName("postback-controller")]
    [AspMvcController, LocalizationRequired(false)]
    public string? Controller {get; set;}
    
    [HtmlAttributeName("postback-method")]
    public HttpMethodEnum? MethodEnum {get; set;}
    
    [HtmlAttributeName(DictionaryAttributePrefix = "postback-param-")]
    public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();
    
    [ViewContext]
    [HtmlAttributeNotBound]
    public required ViewContext ViewContext { get; set; }
    
    private readonly IUrlHelperFactory _urlFactory;

    public PostbackTagHelper(IUrlHelperFactory urlHelperFactory)
    {
        _urlFactory = urlHelperFactory;
    }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var urlHelper = _urlFactory.GetUrlHelper(ViewContext);
        
        output.Attributes.SetAttribute("data-postback-parameter", ParameterName);
        
        if (MethodEnum.HasValue) output.Attributes.SetAttribute("data-postback-method", MethodEnum.Value.ToString());

        //if the method is get then we need to add the request data to the url instead of the body
        if (MethodEnum == HttpMethodEnum.GET)
        {
            output.Attributes.SetAttribute("data-postback", urlHelper.Action(Action, Controller, PageUrlValues));
        }
        else
        {
            output.Attributes.SetAttribute("data-postback", urlHelper.Action(Action, Controller));

            // Add the request data if any
            if (PageUrlValues.Count == 0) return;
            
            var stringB = new StringBuilder();
            
            foreach (var (key, value) in PageUrlValues)
            {
                stringB.Append(key);
                stringB.Append(':');
                stringB.Append(value);
                stringB.Append(',');
            }
                
            stringB.Remove(stringB.Length - 1, 1);
            
            output.Attributes.SetAttribute("data-postback-request-data", stringB.ToString());
        }
    }
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum HttpMethodEnum
{
    GET,
    POST,
    PUT,
    DELETE
}
