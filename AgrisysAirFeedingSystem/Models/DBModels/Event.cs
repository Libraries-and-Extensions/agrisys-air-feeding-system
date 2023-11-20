namespace AgrisysAirFeedingSystem.Models.DBModels;

public class Event
{
    public int EventId { get; set; } // Primary Key
    public int EntityId { get; set; } // Foreign Key
    public string EventDesc { get; set; }
    public Level Level { get; set; }
}