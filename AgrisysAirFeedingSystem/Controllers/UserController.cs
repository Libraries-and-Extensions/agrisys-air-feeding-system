using System.Net;
using System.Security.Claims;
using AgrisysAirFeedingSystem.Authtication;
using AgrisysAirFeedingSystem.Models.Enums;
using AgrisysAirFeedingSystem.Models.viewModels;
using AgrisysAirFeedingSystem.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AgrisysAirFeedingSystem.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;


    public UserController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    
    [AuthorizeClaim("role:read")]
    public async Task<IActionResult> List()
    {
        //Collect all users
        List<UserListEntryViewModel> users = new();
        
        foreach (var user in _userManager.Users)
        {
            var role = await _userManager.GetRoleEnum(user);
            
            users.Add(new UserListEntryViewModel()
            {
                Identity = user,
                Role = role,
            });
        }
        
        //wont work if the database is recreated when not logging again
        //get the current user
        var currentUser = await _userManager.GetUserAsync(User);
        
        if (currentUser == null) return new ForbidResult();
        
        var userRole = await _userManager.GetRoleEnum(currentUser);

        var hasAssignPermission = User.HasClaim("role:assign", "");

        IEnumerable<RoleEnum> roleOptions;
        
        if (hasAssignPermission)
        {
            var list = (
                from roleEnum in Enum.GetValues<RoleEnum>() 
                where roleEnum.GetRoleLevel() < userRole.GetRoleLevel() 
                select roleEnum
            ).ToList();

            if (userRole == RoleEnum.Admin) list.Insert(0,RoleEnum.Admin);

            roleOptions = list;
        }
        else
        {
            roleOptions = Array.Empty<RoleEnum>();
        }

        //return the view
        return View(new UserListViewModel()
        {
            roleOptions = roleOptions,
            CurrentUser = currentUser,
            Users = users,
            CurrentUserRole = userRole,
            hasUpdatePermission = hasAssignPermission,
        });
    }
}