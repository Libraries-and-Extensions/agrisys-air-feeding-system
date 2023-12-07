using System.ComponentModel.DataAnnotations;
using AgrisysAirFeedingSystem.Controllers;

namespace AgrisysAirFeedingSystem.Models.DBModels;

public class Sensor
{
    [Key] public int SensorId { get; set; } // Primary Key

    public int EntityId { get; set; } // Foreign Key
    public Entity Entity { get; set; }
    public string Name { get; set; }    
    public int? min { get; set; }
    public int? max { get; set; }
    
    // Navigational property for sensorvalues
    public ICollection<SensorMeasurement> SensorValues { get; set; }
    public SensorType SensorType { get; set; }
}