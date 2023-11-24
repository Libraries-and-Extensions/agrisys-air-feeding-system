using System.ComponentModel.DataAnnotations;

namespace AgrisysAirFeedingSystem.Models.DBModels;

public class SensorMeasurement
{
    [Key]
    public int MeasurementId { get; set; } // Primary Key
    public int SensorId { get; set; } // Foreign Key
    public int Value { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public Sensor Sensor { get; set; }
}