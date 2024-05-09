using FP.Mqtt.WeatherClient.Business;
using FP.Mqtt.WeatherClient.Components;
using FP.Mqtt.WeatherClient.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddMudServices();

builder.Services.AddSingleton<ICrawlerRepository, CrawlerRepository>();
builder.Services.AddSingleton<IMqttRepository, MqttRepository>();

builder.Services.AddHttpClient("open-meteo", c=>
{
    c.BaseAddress = new Uri("https://api.open-meteo.com/v1/dwd-icon");
});

builder.Services.AddHostedService<WeatherCrawlerService>();
builder.Services.AddHostedService<MqttClientService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
