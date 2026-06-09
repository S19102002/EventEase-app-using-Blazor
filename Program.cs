using EventEase.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<UserSessionService>();

var app = builder.Build();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();
