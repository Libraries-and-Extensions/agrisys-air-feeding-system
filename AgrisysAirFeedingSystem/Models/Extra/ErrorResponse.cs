using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace AgrisysAirFeedingSystem.Models.Extra;

public class ErrorResponse : ActionResult
{
    private string msg { get; set; }
    private int statusCode { get; set; }
    
    public ErrorResponse(string msg, HttpStatusCode statusCode) : this(msg,(int) statusCode)
    {
    }
    
    public ErrorResponse(string msg, int statusCode) 
    {
        this.msg = msg;
        this.statusCode = statusCode;
    }
    
    public override void ExecuteResult(ActionContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var httpContext = context.HttpContext;
        
        httpContext.Response.StatusCode = statusCode;
        
        var response = new {msg};
        
        httpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(response.ToJson()));
    }
}