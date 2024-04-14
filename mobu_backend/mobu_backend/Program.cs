using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using mobu_backend;
using mobu_backend.Data;
using mobu_backend.Hubs.Chat;
using mobu_backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("mobuConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddErrorDescriber<PortugueseIdentityErrorDescriber>();
builder.Services.AddControllersWithViews();

//Política de login
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("OnlyLoggedIn", policy =>
//        policy.AddRequirements()
//});

// Adicionar envio de emails
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

// Adicionar SignalR
builder.Services.AddSignalR(cfg => cfg.EnableDetailedErrors = true);

// Adicionar servicos do Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890áéíóúàèìòùâêîôûãõäëïöüñç '-";
});

// Adicionar configuracao de cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookies
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithHeaders()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials());
}
else
{
    app.UseMigrationsEndPoint();

    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithHeaders()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials());
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.MapHub<RealTimeHub>("hub/RealTimeHub");

app.Run();
