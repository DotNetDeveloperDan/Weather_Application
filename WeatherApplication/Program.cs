using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WeatherApplication.Data;
using WeatherApplication.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ToastService>();
builder.Services.AddHttpClient<WeatherService>();

// Add authentication state provider
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

// Configure Serilog
builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.File("Logs/log.txt");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Ensure authentication middleware is enabled
app.UseAuthorization();   // Ensure authorization middleware is enabled

app.UseMiddleware<ExceptionHandlingMiddleware>();  // Ensure custom middleware is placed correctly
app.MapRazorPages();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();