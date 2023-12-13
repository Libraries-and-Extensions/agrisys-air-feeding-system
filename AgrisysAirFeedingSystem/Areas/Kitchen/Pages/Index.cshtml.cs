using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AgrisysAirFeedingSystem.Areas.Kitchen.Pages;

public class IndexModel : PageModel
{
    public DbSet<Silo> Silos;
    public int Id { get; set; }
    public IndexModel()
    {
        
    }
    public void OnGet()
    {
        Silos = new AgrisysDbContext().Set<Silo>();
        Id = int.Parse( Request.Query["kitchenid"]) - 1;
        Console.WriteLine(Id.ToString());
    }

}