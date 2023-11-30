using System.ComponentModel.DataAnnotations;

namespace AgrisysAirFeedingSystem.Models.DBModels;

// Farm entity
public class Farm
{
    [Key] public int FarmId { get; set; } // Primary Key

    public string Owner { get; set; }
    public string Address { get; set; }
}