using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Steeltoe.Extensions.Configuration.Placeholder;
using Typesense;
using Typesense.Setup;
using WorkShopUI.Clients;
using WorkShopUI.Data;
using WorkShopUI.Services;
using WorkShopUI.SearchSchema;

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

// Add Firestore DB
builder.Services.AddSingleton<FirestoreDb>(config => {

    var firebaseProject = builder.Configuration["Firebase:Project"];
    return FirestoreDb.Create(firebaseProject);
});

// Add typesense client
builder.Services.AddTypesenseClient(config => {

    var host = builder.Configuration["Typesense:Host"];
    var port = builder.Configuration["Typesense:Port"];
    var protocol = builder.Configuration["Typesense:Protocol"];
    var apiKey = builder.Configuration["Typesense:ApiKey"];

    config.ApiKey = apiKey;
    config.Nodes = new List<Node>
    {
        new Node(host, port, protocol)
    };
});

// Build typesense schemas
builder.Services.AddSingleton<SchemaBuilder>();

// HTTP Clients
builder.Services.AddScoped<CarBrandClient>();
builder.Services.AddScoped<CarLineClient>();
builder.Services.AddScoped<ContactClient>();

// Services
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<CarBrandService>();
builder.Services.AddScoped<CarLineService>();
builder.Services.AddScoped<ContactService>();

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

var schemaBuilder = app.Services.GetService<SchemaBuilder>();
schemaBuilder.BuildSchema();


app.Run();
