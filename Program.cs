using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectiveMirror.Data;
using ReflectiveMirror.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<ReflectiveMirrorContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReflectiveMirrorContext")
                         ?? throw new InvalidOperationException("Connection string 'ReflectiveMirrorContext' not found.")));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Initialize database seed data
    SeedData.Initialize(services);

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    var roles = new[] { "User", "Admin" };

    foreach (var role in roles)
    {
        if (!roleManager.RoleExistsAsync(role).Result)
        {
            roleManager.CreateAsync(new IdentityRole(role)).Wait();
        }
    }

    string adminEmail = "admin@reflectivemirrors.com";
    string password = builder.Configuration.GetValue<string>("AdminPassword") ?? "DEFAULT_pa$$word2024";

    if (userManager.FindByEmailAsync(adminEmail).Result == null)
    {
        var admin = new IdentityUser
        {
            Email = adminEmail,
            UserName = adminEmail,
            EmailConfirmed = true
        };

        var result = userManager.CreateAsync(admin, password).Result;
        if (result.Succeeded)
        {
            userManager.AddToRoleAsync(admin, "Admin").Wait();
        }
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Add UseAuthentication before UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
