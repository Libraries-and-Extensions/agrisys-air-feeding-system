using System.ComponentModel.DataAnnotations.Schema;
using AgrisysAirFeedingSystem.Models.Extra;

namespace AgrisysAirFeedingSystem.Models.DBModels;

public class Kitchen
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; }
    [NotMapped]
    public KitchenState State { get; set; }
}