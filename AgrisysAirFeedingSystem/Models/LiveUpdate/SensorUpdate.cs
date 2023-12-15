using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgrisysAirFeedingSystem.Models.LiveUpdate;

public class SensorUpdate
{
    public string Key { get; set; }
    public int Value { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}