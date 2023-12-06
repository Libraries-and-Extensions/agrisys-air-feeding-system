using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Models.DBModels;
using AgrisysAirFeedingSystem.Models.viewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace AgrisysAirFeedingSystem.Controllers;

public class KitchenController : Controller
{
    private readonly AgrisysDbContext _context;

    public KitchenController(AgrisysDbContext context)
    {
        _context = context;
    }
    
    public IActionResult List()
    {
        var Kitchens = _context.Kitchens.ToList();
        List<KitchenViewModel> viewModels = new();
        
        foreach (var kitchen in Kitchens)
        {
            var sensorsData = from sensor in _context.Sensors
                join e in _context.Entities on sensor.EntityId equals e.EntityId
                join g in _context.Groups on e.GroupId equals g.GroupId
                join m in _context.Measurements on sensor.SensorId equals m.SensorId
                where g.GroupId == kitchen.GroupId && sensor.SensorType == SensorType.Status
                orderby sensor.SensorId descending
                select new
                {
                    e,
                    m
                };

            
            var sensorData = sensorsData.FirstOrDefault();

            Console.WriteLine(sensorData.e.Name+ " " + (SensorStatus)sensorData.m.Value);
            
            viewModels.Add(new KitchenViewModel
            {
                kitchen = kitchen,
                statusMsg = sensorData.e.Name+ " " + (SensorStatus)sensorData.m.Value,
                OperationMsg = "Not implemented"
            });
        }
        
        return View(viewModels);
    }
    
    public IActionResult Index()
    {
        return View();
    }
}