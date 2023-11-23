using System.ComponentModel.DataAnnotations;

namespace AgrisysAirFeedingSystem.Models.DBModels;

public class Group
{
    [Key]
    public int GroupId { get; set; } // Primary Key
    public GroupType GroupType { get; set; }
    
    // Navigational property for entities
    public ICollection<Entity> Entities { get; set; }
}