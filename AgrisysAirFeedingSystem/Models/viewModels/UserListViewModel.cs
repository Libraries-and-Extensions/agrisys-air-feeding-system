using AgrisysAirFeedingSystem.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace AgrisysAirFeedingSystem.Models.viewModels;

public class UserListEntryViewModel
{
    public IdentityUser Identity { get; set; }
    public RoleEnum Role { get; set; }
}


public class UserListViewModel
{
    public IEnumerable<UserListEntryViewModel> Users { get; set; }
    
    public IdentityUser CurrentUser { get; set; }
    public RoleEnum CurrentUserRole { get; set; }
    
    public IEnumerable<RoleEnum> roleOptions { get; set; }

    public bool hasUpdatePermission { get; set; } = false;
}
