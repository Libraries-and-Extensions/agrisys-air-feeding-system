namespace AgrisysAirFeedingSystem.Models.DBModels;

public class Data
{
    public int DataId { get; set; } // Primary Key
    public int SensorId { get; set; } // Foreign Key
    public int Value { get; set; }
}