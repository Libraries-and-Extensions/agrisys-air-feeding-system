using AgrisysAirFeedingSystem.Models.DBModels;

namespace AgrisysAirFeedingSystem.Models.viewModels;

public class KitchenViewModel
{
    public Kitchen kitchen { get; set; }
    public string statusMsg { get; set; }
    public string OperationMsg { get; set; }
}