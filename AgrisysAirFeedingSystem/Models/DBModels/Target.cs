using System.ComponentModel.DataAnnotations;

namespace AgrisysAirFeedingSystem.Models.DBModels;

public class Target
{
    [Key]
    public int TargetId { get; set; } // Primary Key
    public int PressureAlarm { get; set; }
    public int BlowerStrength { get; set; }
    public int Cellesluse { get; set; }
    public int BeforeOpen { get; set; }
    public int BeforeClose { get; set; }
    
    public int pigCount { get; set; }
    
    public int pigAge { get; set; }
    
    public int MixtureId { get; set; } // Foreign Key
    public Mixture Mixture { get; set; } // Foreign Key
    
    public int AreaID { get; set; }
    public Area Area { get; set; }
}