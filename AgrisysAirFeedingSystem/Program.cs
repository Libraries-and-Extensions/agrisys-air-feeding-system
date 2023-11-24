using AgrisysAirFeedingSystem.Authtication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AgrisysAirFeedingSystem.Data;
using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Models.Seeding;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AgrisysDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSingleton<IAuthorizationHandler, SimpleClaimAuthHandler>();

builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

var app = builder.Build();

AgrisysDBSeeder.Seed(app);
AuthorizationSeed.Seed(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapHub<SensorHub>("/SensorHub");

app.Run();