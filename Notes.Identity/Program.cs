using Notes.Identity;
using Notes.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Notes.Identity.Models;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddPersistance(builder.Configuration);

builder.Services.AddDbContext<AuthDbContext>(options => 
    options.UseSqlite(builder.Configuration
        .GetValue<string>("DbConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(appUserConfiguration => 
{
    appUserConfiguration.Password.RequiredLength = 4;
    appUserConfiguration.Password.RequireDigit = false;
    appUserConfiguration.Password.RequireNonAlphanumeric = false;
    appUserConfiguration.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<AppUser>()
    .AddInMemoryApiResources(Configuration.ApiResources)
    .AddInMemoryIdentityResources(Configuration.IdentityResources)
    .AddInMemoryApiScopes(Configuration.ApiScopes)
    .AddInMemoryClients(Configuration.Clients)
    .AddDeveloperSigningCredential();

builder.Services.ConfigureApplicationCookie(appCookieConfiguration =>
{
    appCookieConfiguration.Cookie.Name = "Notes.Identity.Cookie";
    appCookieConfiguration.LoginPath = "/Auth/Login";
    appCookieConfiguration.LogoutPath = "/Auth/Logout";

});


builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception) { }
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Styles")),
    RequestPath = "/styles"
});
app.UseRouting();
app.UseIdentityServer();
app.UseEndpoints(endpoints => 
{
    endpoints.MapDefaultControllerRoute();
});

app.MapGet("/", () => "Hello World!");

app.Run();
