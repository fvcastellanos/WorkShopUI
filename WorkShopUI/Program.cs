using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Steeltoe.Extensions.Configuration.Placeholder;
using WorkShopUI.Clients;
using WorkShopUI.Data;
using WorkShopUI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddPlaceholderResolver();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient(ClientConstants.ClientName, config => {

    var apiUrl = builder.Configuration["WorkShopAPI:BaseURL"];
    config.BaseAddress = new Uri(apiUrl); // need to use an environment variable

});

// HTTP Clients
builder.Services.AddScoped<CarBrandClient>();
builder.Services.AddScoped<CarLineClient>();

// Services
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<CarBrandService>();
builder.Services.AddScoped<CarLineService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
