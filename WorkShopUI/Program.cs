using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using WorkShopUI.Clients;
using WorkShopUI.Data;
using WorkShopUI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient("workshop-api", config => {

    config.BaseAddress = new Uri("http://localhost:8080/v1/workshop"); // need to use an environment variable

    var contentType = "Content-Type";
    if (config.DefaultRequestHeaders.Contains(contentType))
    {
        config.DefaultRequestHeaders.Remove(contentType);
    }            

    config.DefaultRequestHeaders.Add(contentType, "application/json");

});

// HTTP Clients
builder.Services.AddScoped<CarBrandClient>();

// Services
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<CarBrandService>();

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
