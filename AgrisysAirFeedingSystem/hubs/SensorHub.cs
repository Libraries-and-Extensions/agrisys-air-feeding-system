using AgrisysAirFeedingSystem.hubs;
using AgrisysAirFeedingSystem.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AgrisysAirFeedingSystem.Hubs;

[Authorize]
public class SensorHub : BaseLiveUpdateHub
{
    
}