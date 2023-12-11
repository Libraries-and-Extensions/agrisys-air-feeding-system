using System.Net;
using System.Security.Claims;
using AgrisysAirFeedingSystem.Authtication;
using AgrisysAirFeedingSystem.Models.apiModels;
using AgrisysAirFeedingSystem.Models.Enums;
using AgrisysAirFeedingSystem.Models.Extra;
using AgrisysAirFeedingSystem.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AgrisysAirFeedingSystem.Controllers.api;

[ApiController]
[Route("api/user")]
public class UserApiController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserApiController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    
    [HttpPost]
    [AuthorizeClaim("role:assign")]
    public async Task<IActionResult> ChangeRole([FromBody]ChangeRoleModel data)
    {
        Console.WriteLine("ChangeRole");
        
        // Check if the user is authorized to change the role of the user
        //if (string.IsNullOrEmpty(data.Id) || string.IsNullOrEmpty(data.Role)) return StatusCode((int)HttpStatusCode.BadRequest);
        
        Console.WriteLine("Adding {0} to user {1}", data.Role,data.Id);

        // get the user to change
        var user = await _userManager.FindByIdAsync(data.Id);
        var loggedInUser = await _userManager.GetUserAsync(User);
        
        // Check if the user exists
        if (user == null) return  new ErrorResponse("Unable to find target user",HttpStatusCode.NotFound);
        if (loggedInUser == null) return new ErrorResponse("Unable to find logged in user",HttpStatusCode.NotFound);
        // Check if the user is trying to change its own role
        if (user == loggedInUser) return new ErrorResponse("Its forbidden to change own role",HttpStatusCode.Forbidden);
        
        // Check if the user is authorized to change the role of the user
        var authorizedUserRoleEnum = await _userManager.GetRoleEnum(loggedInUser);
        var authorizedUserRoleLevel = authorizedUserRoleEnum.GetRoleLevel();
        
        // Check if the role is valid
        if (!Enum.TryParse(typeof(RoleEnum), data.Role, out var roleEnum)) return new ErrorResponse("Invalid role",HttpStatusCode.BadRequest);
        if (!Enum.TryParse(typeof(RoleEnum), data.OldRole, out var oldRoleEnum)) return new ErrorResponse("Invalid oldRole",HttpStatusCode.BadRequest);
        
       // Check if the user is authorized to change the role of the user by compare if the logged in user has a higher role than the user to change
        if (authorizedUserRoleEnum != RoleEnum.Admin && (
                ((RoleEnum)oldRoleEnum).GetRoleLevel() >= authorizedUserRoleLevel 
                || authorizedUserRoleLevel < ((RoleEnum)roleEnum).GetRoleLevel()
                )
            ) 
            return new ErrorResponse("Unable to change due to insufficient permissions",HttpStatusCode.Forbidden);

        var result = IdentityResult.Success;
        
        if ((RoleEnum)roleEnum != RoleEnum.None)
        {
            result = await _userManager.AddToRoleAsync(user, data.Role);
        }
        
        if ((RoleEnum)oldRoleEnum != RoleEnum.None && result.Succeeded)
        {
            result = await _userManager.RemoveFromRoleAsync(user, data.OldRole);
        }
       
        
        return !result.Succeeded 
            ? StatusCode((int)HttpStatusCode.InternalServerError) 
            : StatusCode((int)HttpStatusCode.NoContent);
    }
}