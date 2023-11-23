using System.ComponentModel.DataAnnotations;

namespace AgrisysAirFeedingSystem.Models.DBModels;

public class FeedingTime
{
    [Key]
    public int FeedingTimeId { get; set; } // Primary Key
    public DateTime Time { get; set; }
    public int AreaID { get; set; }
    public Area Area { get; set; }
}