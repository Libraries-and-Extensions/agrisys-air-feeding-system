using Microsoft.AspNetCore.SignalR;

public class SensorHub : Hub
{
    public override async Task OnConnectedAsync()
    {   
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