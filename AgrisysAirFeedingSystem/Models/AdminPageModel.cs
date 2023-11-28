using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AgrisysAirFeedingSystem.Models;

[Authorize(Roles = "Admin")]
public class AdminPageModel : PageModel
{
    
}