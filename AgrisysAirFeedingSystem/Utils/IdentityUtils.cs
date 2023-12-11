using System.Security.Claims;
using System.Security.Principal;
using AgrisysAirFeedingSystem.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace AgrisysAirFeedingSystem.Utils;

public static class IdentityUtils
{
    public static RoleEnum GetRoleEnum(this ClaimsIdentity user)
    {
        return user.FindAll(ClaimTypes.Role).ToList().Aggregate(RoleEnum.None,
            (current, claim) =>
            {
                if (Enum.TryParse(typeof(RoleEnum), claim.Value, out var role) && (int)role < (int)current)
                {
                    return (RoleEnum)role;
                }

                return current;
            });
    }
    
    
    public static async Task<RoleEnum> GetRoleEnum<T>(this UserManager<T> userManager, T user) where T : IdentityUser
    {
        return  (await userManager.GetRolesAsync(user)).Aggregate(RoleEnum.None,
            (current, roleStr) =>
            {
                if (Enum.TryParse(typeof(RoleEnum), roleStr, out var role) && (int)role < (int)current)
                {
                    return (RoleEnum)role;
                }

                return current;
            });
    }
}