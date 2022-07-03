using Auth0.AspNetCore.Authentication;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Steeltoe.Extensions.Configuration.Placeholder;
using WorkShopUI.Clients;
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
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient(ClientConstants.ClientName, config => {

    var apiUrl = builder.Configuration["WorkShopAPI:BaseURL"];
    config.BaseAddress = new Uri(apiUrl);
});

builder.Services.AddAuth0WebAppAuthentication(options => {

    options.ResponseType = "code";
    options.Scope = "openid profile email";
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];

}).WithAccessToken(options => {
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.UseRefreshTokens = true;
});

// HTTP Clients
builder.Services.AddScoped<CarBrandClient>();
builder.Services.AddScoped<CarLineClient>();
builder.Services.AddScoped<ContactClient>();
builder.Services.AddScoped<ProductClient>();

// Services
builder.Services.AddScoped<CarBrandService>();
builder.Services.AddScoped<CarLineService>();
builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<ProductService>();

// Blazorise
builder.Services.AddBlazorise( options => {
    options.Immediate = true;
})
.AddBootstrap5Providers()
.AddFontAwesomeIcons();

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
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
