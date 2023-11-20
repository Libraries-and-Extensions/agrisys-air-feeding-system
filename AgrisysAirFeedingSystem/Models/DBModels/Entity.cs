namespace AgrisysAirFeedingSystem.Models.DBModels;

public class Entity
{
    public int EntityId { get; set; } // Primary Key
    public int GroupId { get; set; } // Foreign Key
    public EntityType EntityType { get; set; }
    public string Name { get; set; }
}