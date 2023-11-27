using System.Diagnostics;
using AgrisysAirFeedingSystem.Authtication;
using Microsoft.AspNetCore.Mvc;
using AgrisysAirFeedingSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace AgrisysAirFeedingSystem.Controllers;

[Authorize()]
public class AdminController : Controller
{
    public AdminController(ILogger<HomeController> logger)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}