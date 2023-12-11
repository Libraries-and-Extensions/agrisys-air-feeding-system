using System.Net;
using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Models.DBModels;
using AgrisysAirFeedingSystem.Models.Extra;
using AgrisysAirFeedingSystem.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AgrisysAirFeedingSystem.Controllers.api;

public class EventController : ApiBaseController
{
    private readonly AgrisysDbContext _dbContext;

    public EventController(AgrisysDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    public IActionResult AcceptEvent(int EntityID, string msg, EditLevel level)
    {
        var entity = _dbContext.Entities.FirstOrDefault(e => e.EntityId == EntityID);
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
        
        _dbContext.Events.Add(eventEntry);
        _dbContext.SaveChanges();
        
        return StatusCode((int)HttpStatusCode.NoContent);
    }
}