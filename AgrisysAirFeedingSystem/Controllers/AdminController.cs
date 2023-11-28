using System.Diagnostics;
using AgrisysAirFeedingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgrisysAirFeedingSystem.Controllers;

[Authorize(Roles = "Admin")]
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