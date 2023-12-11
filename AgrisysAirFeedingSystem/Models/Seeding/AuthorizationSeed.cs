using System.Security.Claims;
using AgrisysAirFeedingSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AgrisysAirFeedingSystem.Models.Seeding;

public class AuthorizationSeed
{
    private static readonly string[] _userClaims =
    {
        "mixture:read", "mixture:update", "log:read", "sensor:temperature", "sensor:humidity", "sensor:weight",
        "sensor:pressure", "sensor:gas"
    };

    private static readonly string[] _managerClaims =
    {
        "mixture:create", "mixture:delete", "feeding_time:read", "feeding_time:create", "feeding_time:update",
        "feeding_time:delete", "manual_control", "role:read", "role:assign"
    };

    private static readonly string[] _adminClaims =
    {
        "user:create", "user:read", "user:update", "user:delete", "role:create", "role:update",
        "role:delete", "farm_database:read", "farm_database:update", "farm_database:delete",
        "farm_database:create", "farm_database:read"
    };

    public static async void Seed(IApplicationBuilder app)
    {
        // Get a scope to get the services
        var scope = app.ApplicationServices.CreateScope().ServiceProvider;

        //get the db context
        var context = scope.GetRequiredService<ApplicationDbContext>();

        //check if there are any pending migrations
        if ((await context.Database.GetPendingMigrationsAsync()).Any()) await context.Database.MigrateAsync();

        var userManager = scope.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.GetRequiredService<RoleManager<IdentityRole>>();

        //check if there are any roles
        if (roleManager.Roles.Any()) return;
        //create roles
        var roles = new List<IdentityRole>
        {
            new("Admin"),
            new("Manager"),
            new("User")
        };

        foreach (var role in roles) await roleManager.CreateAsync(role);

        string[][] allClaims = { _userClaims, _managerClaims, _adminClaims };

        //Add all claims to subsequent roles
        foreach (var claims in allClaims)
        {
            //add all claims to all current roles
            foreach (var claim in claims)
            foreach (var role in roles)
                await roleManager.AddClaimAsync(role, new Claim(claim, ""));
            //remove the last role since it is lower in the hierarchy
            roles.RemoveAt(roles.Count - 1);
        }

        //create admin user
        var admin = new IdentityUser { UserName = "Admin" };
        var result = await userManager.CreateAsync(admin, "Password123!");

        if (result.Succeeded)
            result = await userManager.SetEmailAsync(admin, "marcjensenvirklund@gmail.com");
        if (result.Succeeded)
            result = await userManager.AddToRoleAsync(admin, "Admin");
        if (result.Succeeded)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(admin);
            result = await userManager.ConfirmEmailAsync(admin, token);
        }


        if (!result.Succeeded) throw new Exception("Failed to create admin user");
    }
}