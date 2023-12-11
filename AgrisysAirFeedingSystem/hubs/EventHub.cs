using AgrisysAirFeedingSystem.hubs;
using AgrisysAirFeedingSystem.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class EventHub : BaseLiveUpdateHub
{
}