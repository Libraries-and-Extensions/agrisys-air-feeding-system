using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace AgrisysAirFeedingSystem.Models.Extra;

public class ErrorResponse : ActionResult
{
    public string Msg { get; set; }
    public int StatusCode { get; set; }
    
    public ErrorResponse(string msg, HttpStatusCode statusCode) : this(msg,(int) statusCode)
    {
    }
    
    public ErrorResponse(string msg, int statusCode) 
    {
        this.Msg = msg;
        this.StatusCode = statusCode;
    }
    
    public override void ExecuteResult(ActionContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var httpContext = context.HttpContext;
        
        httpContext.Response.StatusCode = StatusCode;
        
        var response = new { msg = Msg};
        
        httpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(response.ToJson()));
    }
}