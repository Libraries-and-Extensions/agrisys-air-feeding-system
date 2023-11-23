using System.Net;
using AgrisysAirFeedingSystem.Data;
using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Models.DBModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AgrisysAirFeedingSystem.Controllers;

public class SensorController : Controller
{
    private readonly ILogger<SensorController> _logger;
    private readonly IHubContext<SensorHub> _hubContext;
    private readonly AgrisysDbContext _dbContext;

    public SensorController(ILogger<SensorController> logger, IHubContext<SensorHub> hubContext, AgrisysDbContext dbContext)
    {
        _logger = logger;
        _hubContext = hubContext;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> collect(int key, int value)
    {   
        await _hubContext.Clients.Group(key.ToString()).SendAsync("valueUpdate",value);

        _dbContext.Measurements.Add(new SensorMeasurement()
        {
            SensorId = key,
            Value = value,
        });

        await _dbContext.SaveChangesAsync();
        
        return Ok();
    }
}