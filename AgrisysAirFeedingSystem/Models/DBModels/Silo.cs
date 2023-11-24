using System.ComponentModel.DataAnnotations;

namespace AgrisysAirFeedingSystem.Models.DBModels;

public class Silo
{
    [Key] public int SiloId { get; set; } // Primary Key
    public int Capacity { get; set; }
    public int AlarmMin { get; set; }
    public int AfterRun { get; set; }
    public int TransferSpeed { get; set; }
    public int MixingTime { get; set; }
    public int? AlternativeSiloId { get; set; }
    public Silo? AlternativeSilo { get; set; }
}