using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class SensorHub : Hub
{
    public override async Task OnConnectedAsync()
    {   
        //string keys = Context.GetHttpContext().Request.Query["keys"];
        
        //string[] keyArray = keys.Split(';');
        
        Console.WriteLine(Context.GetHttpContext().Request.Query["key"]);
        await Groups.AddToGroupAsync(Context.ConnectionId, Context.GetHttpContext().Request.Query["key"]);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.GetHttpContext().Request.Query["key"]);
        await base.OnDisconnectedAsync(exception);
    }
}