using AgrisysAirFeedingSystem.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class SensorHub : Hub
{
    public override async Task OnConnectedAsync()
    {   
        var context = Context.GetHttpContext();
        
        if (context is null)
        {
            Console.WriteLine("missing https context");
            return;
        }

        foreach (var key in QueryUtils.getListParameter(context, "keys"))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, key);
        }
       
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception != null)
        {
            Console.WriteLine(exception.Message);
            return;
        }
        
        var context = Context.GetHttpContext();
        
        if (context is null)
        {
            Console.WriteLine("missing https context");
            return;
        }

        foreach (var key in QueryUtils.getListParameter(context, "keys"))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, key);
        }

        await base.OnDisconnectedAsync(exception);
    }
}