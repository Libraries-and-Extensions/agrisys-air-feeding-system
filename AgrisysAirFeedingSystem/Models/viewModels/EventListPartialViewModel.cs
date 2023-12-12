using AgrisysAirFeedingSystem.Models.DBModels;

namespace AgrisysAirFeedingSystem.Models.viewModels;

public class EventListPartialViewModel
{
    public IQueryable<Event> Events { get; set; }
    public EditLevel? Level { get; set; }
    public int offset { get; set; }
}