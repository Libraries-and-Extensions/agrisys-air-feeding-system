using System.ComponentModel.DataAnnotations;

namespace AgrisysAirFeedingSystem.Models.DBModels;

public class Entity
{
    [Key]
    public int EntityId { get; set; } // Primary Key
    public int GroupId { get; set; } // Foreign Key
    public Group Group { get; set; }
    public EntityType EntityType { get; set; }
    public string Name { get; set; }
    
    // Navigational property for events, sensors
    public ICollection<Event> Events { get; set; }
    public ICollection<Sensor> Sensors { get; set; }
}