﻿using System.Globalization;
using System.Net;
using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Models.DBModels;
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
            var sensorUpdate = new SensorUpdate
            {
                key = key,
                Value = value
            };

            //send to group
            await _hubContext.Clients.Group(sensorUpdate.key).SendAsync("valueUpdate", sensorUpdate);

            //save to db
            _dbContext.Measurements.Add(new SensorMeasurement
            {
                SensorId = int.Parse(key, NumberStyles.HexNumber),
                Value = value,
                TimeStamp = sensorUpdate.TimeStamp
            });

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }


        return Ok();
    }
}