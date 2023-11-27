using System.Net;
using AgrisysAirFeedingSystem.Data;
using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Models.DBModels;
using AgrisysAirFeedingSystem.Models.LiveUpdate;
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
    public async Task<IActionResult> Collect(string key, int value)
    {
        try
        {
            var measurement = new SensorMeasurement()
            {
                SensorId = int.Parse(key, System.Globalization.NumberStyles.HexNumber),
                Value = value,
            };
            
            //check if sensor exists
            var sensor =  _dbContext.Sensors.FirstOrDefault(s => s.SensorId == measurement.SensorId);
            
            if (sensor == null)
            {
                return StatusCode((int) HttpStatusCode.NotFound);
            }
            
            //check if value is in range
            if (sensor.min > measurement.Value || sensor.max < measurement.Value)
            {
                return StatusCode((int) HttpStatusCode.BadRequest);
            }
            
            //create sensor update
            var sensorUpdate = new SensorUpdate()
            {
                key = key,
                Value = value,
                TimeStamp = measurement.TimeStamp,
            };
            
            //send to group
            await _hubContext.Clients.Group(sensorUpdate.key).SendAsync("valueUpdate",sensorUpdate);
            
            //save to db
            _dbContext.Measurements.Add(measurement);

            await _dbContext.SaveChangesAsync();
            
            
        }
        catch (Exception)
        {
            return StatusCode((int) HttpStatusCode.InternalServerError);
        }
        
        
        return Ok();
    }
}