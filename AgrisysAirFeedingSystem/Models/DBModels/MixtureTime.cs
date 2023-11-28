using System.ComponentModel.DataAnnotations;

namespace AgrisysAirFeedingSystem.Models.DBModels;

public class MixtureSetpoint
{
    [Key] public int MixtureSetpointId { get; set; } // Primary Key

    public int age { get; set; }
    public int SetPoint { get; set; }

    public int MixtureId { get; set; }
    public Mixture Mixture { get; set; }
}