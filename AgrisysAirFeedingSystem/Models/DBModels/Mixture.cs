
using System.ComponentModel.DataAnnotations;

namespace AgrisysAirFeedingSystem.Models.DBModels;

public class Mixture
{
    [Key]
    public int MixtureId { get; set; } // Primary Key
    public int FirstSiloId { get; set; } // Foreign Key
    public Silo FirstSilo { get; set; } // Foreign Key
    public int SecondSiloId { get; set; } // Foreign Key
    public Silo SecondSilo { get; set; } // Foreign Key
    
    // Navigational property for mixtures
    public ICollection<Target> UsedForTargets { get; set; }
    public ICollection<MixtureSetpoint> Setpoints { get; set; }
    
}