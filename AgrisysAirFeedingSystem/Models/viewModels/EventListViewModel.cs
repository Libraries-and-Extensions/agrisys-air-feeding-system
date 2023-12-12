using AgrisysAirFeedingSystem.Models.DBModels;

namespace AgrisysAirFeedingSystem.Models.viewModels;

public class EventListViewModel
{
    public IEnumerable<Event> Events { get; set; }
    public EditLevel? Level { get; set; }
    public int PageCount { get; set; }
}