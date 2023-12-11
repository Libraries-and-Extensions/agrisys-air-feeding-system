using System.ComponentModel.DataAnnotations;

namespace AgrisysAirFeedingSystem.Models.DBModels;

public class Event
{
    [Key] public int EventId { get; set; } // Primary Key

    public string EventDesc { get; set; }
    public EditLevel EditLevel { get; set; }

    public int EntityId { get; set; } // Foreign Key

    public DateTime TimeStamp { get; set; } = DateTime.Now;
    // Navigational property
    public Entity Entity { get; set; }
}