using Microsoft.AspNetCore.Mvc;

namespace AgrisysAirFeedingSystem.Controllers;

public class KitchenController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}