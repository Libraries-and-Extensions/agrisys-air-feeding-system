using System.Net;
using AgrisysAirFeedingSystem.Hubs;
using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Models.DBModels;
using AgrisysAirFeedingSystem.Models.Extra;
using AgrisysAirFeedingSystem.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AgrisysAirFeedingSystem.Controllers.api;

public class EventController : ApiBaseController
{
    private readonly AgrisysDbContext _dbContext;
    private IHubContext<EventHub> _hubContext;

    public EventController(IHubContext<EventHub> hubContext,
        AgrisysDbContext dbContext)
    {
        _hubContext = hubContext;
        _dbContext = dbContext;
    }
    
    
    public async Task<IActionResult> Collect(int entityId, string msg, EditLevel level)
    {
        var entity = _dbContext.Entities.Include(e=>e.Group).FirstOrDefault(e => e.EntityId == entityId);
        if (entity == null)
        {
            return new ErrorResponse("Entity not found", HttpStatusCode.NotFound);
        }
        
        var eventEntry = new Event()
        {
            Entity = entity,
            EventDesc = msg,
            EditLevel = level
        };
        
        //create sensor update
        var eventUpdate = new EventUpdate()
        {
            Key = entity.EntityId.ToString(),
            Value = msg,
            EntityName = entity.Name,
            Level = level,
            timestamp = DateTime.Now
        };
        
        //send to group
        await _hubContext.Clients.Group("all").SendAsync("valueUpdate",eventUpdate);
        await _hubContext.Clients.Group(entity.Group.GroupId.ToString("X")).SendAsync("valueUpdate",eventUpdate);
        
        _dbContext.Events.Add(eventEntry);
        await _dbContext.SaveChangesAsync();
        
        return StatusCode((int)HttpStatusCode.NoContent);
    }
}

class EventUpdate
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string EntityName { get; set; }
    public EditLevel Level { get; set; }
    public DateTime timestamp { get; set; }
}