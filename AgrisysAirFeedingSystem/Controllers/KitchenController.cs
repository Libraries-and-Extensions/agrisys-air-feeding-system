using Microsoft.AspNetCore.Mvc;

namespace AgrisysAirFeedingSystem.Controllers;

public class TestKitchenController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}