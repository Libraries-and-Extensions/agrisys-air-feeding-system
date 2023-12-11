using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Models.DBModels;
using AgrisysAirFeedingSystem.Models.viewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgrisysAirFeedingSystem.Controllers;

public class EventController : Controller
{
    private readonly AgrisysDbContext _dbContext;

    public EventController(AgrisysDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IActionResult Index(EditLevel? level)
    {
        Console.WriteLine("level {0}",level);
        
        var models = _dbContext.Events
            .Include(e => e.Entity)
            .Where(e => level == null || e.EditLevel == level).OrderByDescending((e => e.TimeStamp));
        
        return View(new EventListViewModel()
        {
            Events = models,
            Level = level
        });
    }
}