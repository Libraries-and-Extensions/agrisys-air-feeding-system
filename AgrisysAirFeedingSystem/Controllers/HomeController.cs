using System.Diagnostics;
using AgrisysAirFeedingSystem.Authtication;
using Microsoft.AspNetCore.Mvc;
using AgrisysAirFeedingSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace AgrisysAirFeedingSystem.Controllers;

[Authorize()]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    [AuthorizeClaim("Privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}