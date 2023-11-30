using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Models.DBModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AgrisysAirFeedingSystem.Areas.Kitchen.Pages;

public class Index : PageModel
{
    public DbSet<Silo> Silos;
    public void OnGet()
    {
        Silos = new AgrisysDbContext().Set<Silo>();
    }
}