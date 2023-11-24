namespace AgrisysAirFeedingSystem.Models.LiveUpdate;

public class SensorUpdate
{
    public string key { get; set; }
    public int Value { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}