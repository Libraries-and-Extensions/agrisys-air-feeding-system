using Microsoft.AspNetCore.Mvc;

namespace AgrisysAirFeedingSystem.Utils;

[ApiController]
[Route("/Api/[controller]/[action]")]
public abstract class ApiBaseController : Controller { }