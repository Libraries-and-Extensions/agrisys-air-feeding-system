using System.Globalization;
using System.Net;
using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Models.DBModels;
using AgrisysAirFeedingSystem.Models.Extra;
using AgrisysAirFeedingSystem.Models.LiveUpdate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AgrisysAirFeedingSystem.Controllers;

public class SensorController : Controller
{
    private readonly AgrisysDbContext _dbContext;
    private readonly IHubContext<SensorHub> _hubContext;
    private readonly ILogger<SensorController> _logger;

    public SensorController(ILogger<SensorController> logger, IHubContext<SensorHub> hubContext,
        AgrisysDbContext dbContext)
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
                return new ErrorResponse("Sensor Not found", (int) HttpStatusCode.NotFound);
            }
            
            //check if value is in range
            if (sensor.min > measurement.Value || sensor.max < measurement.Value)
            {
                return new ErrorResponse("Value out of range", (int) HttpStatusCode.BadRequest);
            }
            
            //create sensor update
            var sensorUpdate = new SensorUpdate()
            {
                Key = key,
                Value = value,
                TimeStamp = measurement.TimeStamp,
            };

            //send to group
            await _hubContext.Clients.Group(sensorUpdate.Key).SendAsync("valueUpdate",sensorUpdate);
            
            //save to db
            _dbContext.Measurements.Add(measurement);

            await _dbContext.SaveChangesAsync();
            
            
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }


        return Ok();
    }
}