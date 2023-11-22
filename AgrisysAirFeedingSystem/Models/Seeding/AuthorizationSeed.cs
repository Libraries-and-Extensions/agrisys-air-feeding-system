using System.Security.Claims;
using AgrisysAirFeedingSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AgrisysAirFeedingSystem.Models.Seeding;

public class AgrisysDBSeeder
{
    static string[] USerClaims = {"Privacy","User"};
    
    public async static void Seed(IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope().ServiceProvider;
        
        
        ApplicationDbContext context = scope.GetRequiredService<ApplicationDbContext>();
        
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
        
        UserManager<IdentityUser> userManager = scope.GetRequiredService<UserManager<IdentityUser>>();
        RoleManager<IdentityRole> roleManager = scope.GetRequiredService<RoleManager<IdentityRole>>();
        
        
        if (roleManager.Roles.Any()) return;
        
        var roles = new List<IdentityRole>
        {
            new("Admin"),
            new("Manager"),
            new("User")
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var claim in USerClaims)
        {
            foreach (var role in roles)
            {
                await roleManager.AddClaimAsync(role, new Claim(claim, ""));
            }
        }
        
        var admin = new IdentityUser(){ UserName = "marcjensenvirklund@gmail.com", EmailConfirmed = true};
        var result = await userManager.CreateAsync(admin, "Password123!");
        
        if (result.Succeeded)
            result = await userManager.SetEmailAsync(admin, "marcjensenvirklund@gmail.com");
        if (result.Succeeded)
            result = await userManager.AddToRoleAsync(admin, roles[0].Name);
            

        if (!result.Succeeded) throw new Exception("Failed to create admin user");
    }
}