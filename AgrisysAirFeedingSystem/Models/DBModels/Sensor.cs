namespace AgrisysAirFeedingSystem.Models.DBModels;

public class Sensor
{
    public int SensorId { get; set; } // Primary Key
    public int EntityId { get; set; } // Foreign Key
    public string Name { get; set; }
}