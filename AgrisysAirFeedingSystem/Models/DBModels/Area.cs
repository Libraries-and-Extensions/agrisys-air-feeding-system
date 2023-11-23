using System.ComponentModel.DataAnnotations;

namespace AgrisysAirFeedingSystem.Models.DBModels;

public class Area
{
    [Key]
    public int AreaId { get; set; } // Primary Key
    public string Name { get; set; }
    
    // Navigational property for targets and feedingtimes
    public ICollection<Target> Targets { get; set; }
    public ICollection<FeedingTime> FeedingTimes { get; set; }
}