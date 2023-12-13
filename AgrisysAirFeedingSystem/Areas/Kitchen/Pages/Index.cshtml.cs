using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Models.DBModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AgrisysAirFeedingSystem.Areas.Kitchen.Pages;

public class IndexModel : PageModel
{
    public List<Silo> Silos;
    public int Id { get; set; }
    private AgrisysDbContext _context;
    public IndexModel(AgrisysDbContext context)
    {
        _context = context;
    }
    public void OnGet()
    {
        Silos = _context.Silos.ToList();
        Id = int.Parse( Request.Query["kitchenid"]) - 1;
        Console.WriteLine(Id.ToString());
    }

}