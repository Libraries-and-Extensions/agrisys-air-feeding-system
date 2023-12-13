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
    public IActionResult Index(EditLevel? level,int pageCount = 20)
    {
        var models = _dbContext.Events
            .Include(e => e.Entity)
            .Where(e => level == null || e.EditLevel == level)
            .OrderByDescending((e => e.TimeStamp))
            .Take(pageCount);
        
        return View(new EventListViewModel()
        {
            Events = models,
            Level = level,
            PageCount = pageCount
        });
    }
    public IActionResult IndexPartial(int offset,EditLevel? level, int fetchCount = 10)
    {
        Console.WriteLine("level {0}",level);
        
        var models = _dbContext.Events
            .Include(e => e.Entity)
            .Where(e => level == null || e.EditLevel == level)
            .OrderByDescending((e => e.TimeStamp))
            .Skip(offset)
            .Take(fetchCount);
        
        return View(models);
    }
}