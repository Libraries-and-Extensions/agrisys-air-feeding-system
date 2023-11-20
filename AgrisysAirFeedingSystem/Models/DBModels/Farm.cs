namespace AgrisysAirFeedingSystem.Models.DBModels;

// Farm entity
public class Farm
{
    public int FarmId { get; set; } // Primary Key
    public string Owner { get; set; }
    public string Address { get; set; }
}