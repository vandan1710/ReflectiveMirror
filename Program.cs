using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectiveMirror.Data;
using Microsoft.Extensions.DependencyInjection;
using ReflectiveMirror.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ReflectiveMirrorContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReflectiveMirrorContext") ?? throw new InvalidOperationException("Connection string 'ReflectiveMirrorContext' not found.")));
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

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

using (var scope = app.Services.CreateScope())
{
    var rm = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "User", "Admin" };

    foreach (var role in roles)
    {
        if (! await rm.RoleExistsAsync(role))
        {
            await rm.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var um = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string adminEmail = "admin@reflectivemirrors.com";
    string password = builder.Configuration.GetConnectionString("AdminPassword") ?? "DEFAULT_pa$$word2024";

    if (await um.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new IdentityUser
        {
            Email = adminEmail,
            UserName = adminEmail,
            EmailConfirmed = true
        };

        await um.CreateAsync(admin, password);

        await um.AddToRoleAsync(admin, "Admin");
    }
}

app.Run();
